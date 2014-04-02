using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition link</summary>
	public class CXmlError : CXmlBase
	{
		#region Constants
		
		private const string XML_ERROR_ELEMENT_NAME = "error";

		private const string XML_ERROR_ATTRIBUTE_TEXT  = "text";
		private const string XML_ERROR_ATTRIBUTE_FATAL = "fatal";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bounded to the Text property</summary>
		private string m_strText = "";	
		
		/// <summary>This member is bounded to the Fatal property</summary>
		private bool m_bFatal = false;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlError() : base()
		{
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlError">Source object to be copied</param>
		public CXmlError(CXmlError xmlError) : base()
		{
			Debug.Assert(xmlError != null);
			
			if(xmlError != null)
				Copy(xmlError);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				return -1;
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		/// <param name="xmlError">The source object to be copied</param>
		public void Copy(CXmlError xmlError)
		{
			//	Copy the base class members
			base.Copy(xmlError as CXmlBase);
			
			Text  = xmlError.Text;
			Fatal = xmlError.Fatal;
			
		}// public void Copy(CXmlError xmlError)
		
		/// <summary>This method will set the object'properties using the specified node</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlNode xmlNode)
		{
			XPathNavigator xpNavigator = null;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				if((xpNavigator = xmlNode.CreateNavigator()) != null)
					return SetProperties(xpNavigator);
				else
					return false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML error properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the link properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_ERROR_ATTRIBUTE_TEXT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strText = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_ERROR_ATTRIBUTE_FATAL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bFatal = this.XmlToBool(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML error properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method will set the properties using the specified XML text reader</summary>
		/// <param name="xmlReader">The XML text reader used to iterate the file</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlTextReader xmlReader)
		{
			string strAttribute = "";
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.HasAttributes == false) return false;
			
			try
			{
				strAttribute = xmlReader.GetAttribute(XML_ERROR_ATTRIBUTE_TEXT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strText = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_ERROR_ATTRIBUTE_FATAL,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_bFatal = this.XmlToBool(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML error properties using the specified XML text reader", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlReader xmlReader)
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_ERROR_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_ERROR_ATTRIBUTE_TEXT, Text) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_ERROR_ATTRIBUTE_FATAL, Fatal) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Properties
		
		//	The error text message
		public string Text
		{
			get{ return m_strText; }
			set{ m_strText = value; }
			
		}// Text Property
		
		//	True if the error is fatal
		public bool Fatal
		{
			get{ return m_bFatal; }
			set{ m_bFatal = value; }
			
		}// Fatal Property
		
		#endregion Properties
		
	}// public class CXmlError

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlError objects
	/// </summary>
	public class CXmlErrors : CTmaxSortedArrayList
	{
		#region Constants
		
		public const string XML_ERRORS_ELEMENT_NAME = "errors";

		#endregion Constants
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlErrors() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="xmlError">CXmlError object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlError Add(CXmlError xmlError)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlError as object);

				return xmlError;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlError xmlError)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlError">The object to be removed</param>
		public void Remove(CXmlError xmlError)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlError as object);
			}
			catch
			{
			}
		}
		
		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlError">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlError xmlError)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlError as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlError this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlError);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlError value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			XmlNode		xmlChild	= null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_ERRORS_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					//	NO attributes to add
						
					//	Add each of the errors
					foreach(CXmlError O in this)
					{
						if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
						{
							xmlElement.AppendChild(xmlChild);
						}
					}
					
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>The method is called to retrieve the total number of fatal errors in the collection</summary>
		/// <returns>The total number of fatal errors</returns>
		public int GetFatalCount()
		{
			int iFatal = 0;
			
			foreach(CXmlError O in this)
			{
				if(O.Fatal == true)
					iFatal++;
			}
			
			return iFatal;
			
		}// public int GetFatalCount()
		
		#endregion Public Methods
		
	}//	public class CXmlErrors
		
}// namespace FTI.Shared.Xml
