using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition link</summary>
	public class CXmlSecondary : CXmlBase
	{
		#region Constants
		
		private const string XML_SECONDARY_ELEMENT_NAME = "secondary";

		private const string XML_SECONDARY_ATTRIBUTE_PAGE = "page";
		private const string XML_SECONDARY_ATTRIBUTE_PATH = "path";
		private const string XML_SECONDARY_ATTRIBUTE_NAME = "name";
		private const string XML_SECONDARY_ATTRIBUTE_DESCRIPTION = "description";
		private const string XML_SECONDARY_ATTRIBUTE_FOREIGN_BARCODE = "fbc";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bounded to the Node property</summary>
		private XmlNode m_xmlNode = null;
		
		/// <summary>This member is bounded to the Path property</summary>
		private string m_strPath = "";	
		
		/// <summary>This member is bounded to the Name property</summary>
		private string m_strName = "";		
		
		/// <summary>This member is bounded to the Description property</summary>
		private string m_strDescription = "";		
		
		/// <summary>This member is bounded to the ForeignBarcode property</summary>
		private string m_strForeignBarcode = "";		
		
		/// <summary>This member is bounded to the Page property</summary>
		private int m_iPage = 0;		
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CXmlSecondary() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		/// <param name="xmlNode">The XML node used to set this object's properties</param>
		public CXmlSecondary(XmlNode xmlNode) : base()
		{
			m_xmlNode = xmlNode;
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSecondary">Source object to be copied</param>
		public CXmlSecondary(CXmlSecondary xmlSecondary) : base()
		{
			Debug.Assert(xmlSecondary != null);
			
			if(xmlSecondary != null)
				Copy(xmlSecondary);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				if(this.Page == ((CXmlSecondary)xmlBase).Page)
					return 0;
				else if(this.Page > ((CXmlSecondary)xmlBase).Page)
					return 1;
				else
					return -1;
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the peeeeeeeroperties of the source node</summary>
		/// <param name="xmlSecondary">The source object to be copied</param>
		public void Copy(CXmlSecondary xmlSecondary)
		{
			//	Copy the base class members
			base.Copy(xmlSecondary as CXmlBase);
			
			Node = xmlSecondary.Node;
			Page = xmlSecondary.Page;
			Path = xmlSecondary.Path;
			Name = xmlSecondary.Name;
			Description = xmlSecondary.Description;
			ForeignBarcode = xmlSecondary.ForeignBarcode;
			
		}// public void Copy(CXmlSecondary xmlSecondary)
		
		/// <summary>This method will set the object'properties using the specified node</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlNode xmlNode)
		{
			XPathNavigator xpNavigator = null;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				//	Set the node if not already set
				if(m_xmlNode == null)
					m_xmlNode = xmlNode;
				
				if((xpNavigator = xmlNode.CreateNavigator()) != null)
					return SetProperties(xpNavigator);
				else
					return false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML secondary properties", Ex);
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
				strAttribute = xpNavigator.GetAttribute(XML_SECONDARY_ATTRIBUTE_PAGE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					try { m_iPage = System.Convert.ToInt32(strAttribute); }
					catch { m_iPage = 0; }
				}

				strAttribute = xpNavigator.GetAttribute(XML_SECONDARY_ATTRIBUTE_PATH,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strPath = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SECONDARY_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SECONDARY_ATTRIBUTE_DESCRIPTION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDescription = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SECONDARY_ATTRIBUTE_FOREIGN_BARCODE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strForeignBarcode = strAttribute;
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML secondary properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method will set the properties using the XML text reader</summary>
		/// <param name="xpNavigator">The XML text reader positioned on the secondary node</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlTextReader xmlReader)
		{
			string strAttribute = "";
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.HasAttributes == false) return false;
			
			try
			{
				strAttribute = xmlReader.GetAttribute(XML_SECONDARY_ATTRIBUTE_PAGE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
				{
					try { m_iPage = System.Convert.ToInt32(strAttribute); }
					catch { m_iPage = 0; }
				}

				strAttribute = xmlReader.GetAttribute(XML_SECONDARY_ATTRIBUTE_PATH,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strPath = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_SECONDARY_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_SECONDARY_ATTRIBUTE_DESCRIPTION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDescription = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_SECONDARY_ATTRIBUTE_FOREIGN_BARCODE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strForeignBarcode = strAttribute;
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML secondary properties using the XML text reader", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlTextReader xmlReader)
		
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
				strElementName = XML_SECONDARY_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_SECONDARY_ATTRIBUTE_PAGE, Page) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SECONDARY_ATTRIBUTE_PATH, Path) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SECONDARY_ATTRIBUTE_NAME, Name) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SECONDARY_ATTRIBUTE_DESCRIPTION, Description) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SECONDARY_ATTRIBUTE_FOREIGN_BARCODE, ForeignBarcode) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Properties
		
		//	The XML node associated with this object
		public XmlNode Node
		{
			get{ return m_xmlNode; }
			set{ m_xmlNode = value; }
		}
		
		//	The page number associated with the secondary record
		public int Page
		{
			get{ return m_iPage; }
			set{ m_iPage = value; }
		}
		
		//	The path to the secondary record's source file
		public string Path
		{
			get{ return m_strPath; }
			set{ m_strPath = value; }
		}
		
		//	Name assigned to the record in the database
		public string Name
		{
			get{ return m_strName; }
			set{ m_strName = value; }
		}
		
		//	Description assigned to the record in the database
		public string Description
		{
			get{ return m_strDescription; }
			set{ m_strDescription = value; }
		}
		
		//	Foreign barcode assigned to the record in the database
		public string ForeignBarcode
		{
			get{ return m_strForeignBarcode; }
			set{ m_strForeignBarcode = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlSecondary

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlSecondary objects
	/// </summary>
	public class CXmlSecondaries : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlSecondaries() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="xmlSecondary">CXmlSecondary object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlSecondary Add(CXmlSecondary xmlSecondary)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlSecondary as object);

				return xmlSecondary;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlSecondary xmlSecondary)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlSecondary">The object to be removed</param>
		public void Remove(CXmlSecondary xmlSecondary)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlSecondary as object);
			}
			catch
			{
			}
		}
		
		/// <returns>The object with the specified Page value</returns>
		public CXmlSecondary Find(int iPage)
		{
			// Search for the object with the same MediaId
			foreach(CXmlSecondary O in this)
			{
				if(O.Page == iPage)
				{
					return O;
				}
			}
			return null;

		}//	public CXmlSecondary Find(int iPage)

		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlSecondary">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlSecondary xmlSecondary)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlSecondary as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlSecondary this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlSecondary);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlSecondary value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlSecondaries
		
}// namespace FTI.Shared.Xml
