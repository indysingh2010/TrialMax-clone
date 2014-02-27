using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Windows.Forms;

using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition link</summary>
	public class CXmlPrimary : CXmlBase
	{
		#region Constants
		
		private const string XML_PRIMARY_ELEMENT_NAME = "primary";

		private const string XML_PRIMARY_ATTRIBUTE_MEDIA_ID = "mediaid";
		private const string XML_PRIMARY_ATTRIBUTE_NAME = "name";
		private const string XML_PRIMARY_ATTRIBUTE_DESCRIPTION = "description";
		private const string XML_PRIMARY_ATTRIBUTE_FOREIGN_BARCODE = "fbc";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bounded to the XmlNode property</summary>
		private XmlNode m_xmlNode = null;
		
		/// <summary>This member is bounded to the MediaId property</summary>
		private string m_strMediaId = "";	
		
		/// <summary>This member is bounded to the Name property</summary>
		private string m_strName = "";		
		
		/// <summary>This member is bounded to the Description property</summary>
		private string m_strDescription = "";		
		
		/// <summary>This member is bounded to the ForeignBarcode property</summary>
		private string m_strForeignBarcode = "";		
		
		/// <summary>This member is bounded to the Children property</summary>
		private int m_iChildren = 0;
		
		/// <summary>Local member bound to Secondaries property</summary>
		protected CXmlSecondaries m_xmlSecondaries = new CXmlSecondaries();

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CXmlPrimary() : base()
		{
		}
		
		/// <summary>Constructor</summary>
		public CXmlPrimary(XmlNode xmlNode) : base()
		{
			m_xmlNode = xmlNode;
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlPrimary">Source object to be copied</param>
		public CXmlPrimary(CXmlPrimary xmlPrimary) : base()
		{
			Debug.Assert(xmlPrimary != null);
			
			if(xmlPrimary != null)
				Copy(xmlPrimary);
		}
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				return CTmaxToolbox.Compare(this.MediaId, ((CXmlPrimary)xmlBase).MediaId, true);
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to copy the properties of the source node</summary>
		/// <param name="xmlPrimary">The primary object to be copied</param>
		public void Copy(CXmlPrimary xmlPrimary)
		{
			//	Copy the base class members
			base.Copy(xmlPrimary as CXmlBase);
			
			Node = xmlPrimary.Node;
			MediaId = xmlPrimary.MediaId;
			Name = xmlPrimary.Name;
			Description = xmlPrimary.Description;
			ForeignBarcode = xmlPrimary.ForeignBarcode;
			Children = xmlPrimary.Children;
			
		}// public void Copy(CXmlPrimary xmlPrimary)
		
		/// <summary>This method will set the object'properties using the specified node</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlNode xmlNode)
		{
			XPathNavigator xpNavigator = null;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				m_xmlNode = xmlNode;
				
				if((xpNavigator = xmlNode.CreateNavigator()) != null)
					return SetProperties(xpNavigator);
				else
					return false;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML primary properties", Ex);
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
				strAttribute = xpNavigator.GetAttribute(XML_PRIMARY_ATTRIBUTE_MEDIA_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strMediaId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRIMARY_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRIMARY_ATTRIBUTE_DESCRIPTION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDescription = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRIMARY_ATTRIBUTE_FOREIGN_BARCODE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strForeignBarcode = strAttribute;
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML primary properties using the XPath navigator", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method will set the properties using the specified XML text reader</summary>
		/// <param name="xmlReader">The XML text reader used to open the file</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XmlTextReader xmlReader)
		{
			string strAttribute = "";
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.HasAttributes == false) return false;
			
			try
			{
				strAttribute = xmlReader.GetAttribute(XML_PRIMARY_ATTRIBUTE_MEDIA_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strMediaId = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_PRIMARY_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_PRIMARY_ATTRIBUTE_DESCRIPTION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strDescription = strAttribute;

				strAttribute = xmlReader.GetAttribute(XML_PRIMARY_ATTRIBUTE_FOREIGN_BARCODE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strForeignBarcode = strAttribute;
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML primary properties using the XML text reader", Ex);
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
				strElementName = XML_PRIMARY_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_PRIMARY_ATTRIBUTE_MEDIA_ID, MediaId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRIMARY_ATTRIBUTE_NAME, Name) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRIMARY_ATTRIBUTE_DESCRIPTION, Description) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRIMARY_ATTRIBUTE_FOREIGN_BARCODE, ForeignBarcode) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This method is called to populate the secondaries collection</summary>
		/// <param name="xpDocument">The XPath document containing the secondary nodes</param>
		/// <returns>true if successful</returns>
		public bool GetSecondaries(XPathDocument xpDocument)
		{
			XPathNavigator		xpNavigator  = null;
			XPathNodeIterator	xpIterator   = null;
			CXmlSecondary		xmlSecondary = null;
			string				strXPath = "";
			try
			{
				//	Make sure we have a valid primaries collection
				Debug.Assert(m_xmlSecondaries != null);
				m_xmlSecondaries.Clear();
					
				strXPath = String.Format("trialMax/loadfile/primary[@mediaid=\"{0}\"]/secondary", MediaId);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				
				//	Add an object for each primarie
				while(xpIterator.MoveNext() == true)
				{
					xmlSecondary = new CXmlSecondary();
					m_tmaxEventSource.Attach(xmlSecondary.EventSource);
					
					if(xmlSecondary.SetProperties(xpIterator.Current) == true)
					{
						m_xmlSecondaries.Add(xmlSecondary);
					}
						
				}// while(xpIterator.MoveNext() == true)

				//	Update the number of children
				m_iChildren = m_xmlSecondaries.Count;
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSecondaries", "An exception was raised while retreiving secondary elements for " + MediaId, Ex);
				return false;
			}
			
		}
		
		/// <summary>This method is called to populate the secondaries collection</summary>
		/// <param name="xmlReader">The XML text reader used to iterate the file</param>
		///	<param name="bFill">True to fill the collection, otherwise only the count is set</param>
		/// <returns>true if successful</returns>
		public bool GetSecondaries(XmlTextReader xmlReader, bool bFill)
		{
			CXmlSecondary	xmlSecondary = null;
			int				iDepth = 0;
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.EOF == true) return false;

			try
			{
				//	Reset the children
				Debug.Assert(m_xmlSecondaries != null);
				m_xmlSecondaries.Clear();
				this.Children = 0;
				
				//	Get the current depth so that we know when we've run out of children
				//
				//	NOTE:	We assume the reader is positioned on the parent <primary> node
				iDepth = xmlReader.Depth;
				
				//	Read all secondary nodes
				while(AdvanceReader(xmlReader, false) == true)
				{
					//	Is it time to break out?
					if(xmlReader.Depth <= iDepth)
						break;

					//	Don't bother if not an element
					if(xmlReader.NodeType != XmlNodeType.Element) continue;
					
					//	Are we populating the collection?
					if(bFill == true)
					{
						xmlSecondary = new CXmlSecondary();
						m_tmaxEventSource.Attach(xmlSecondary.EventSource);
						
						if(xmlSecondary.SetProperties(xmlReader) == true)
							m_xmlSecondaries.Add(xmlSecondary);
						else
							xmlSecondary = null;
					}
					else
					{
						//	Keep track of the number of child secondaries
						this.Children++;
					}
					
				}// while(AdvanceReader(xmlReader, false) == true)

				//	Update the number of children if we filled the collection
				if(bFill == true)
				{
					m_xmlSecondaries.Sort(true);
					this.Children = m_xmlSecondaries.Count;
				}
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetSecondaries", "An exception was raised while retreiving secondary elements for " + MediaId, Ex);
				return false;
			}
			
		}// public bool GetSecondaries(XmlTextReader xmlReader, bool bFill)
		
		/// <summary>This method is called to populate the secondaries collection</summary>
		/// <returns>true if successful</returns>
		public bool GetSecondaries()
		{
			CXmlSecondary xmlSecondary = null;
			
			//	This method requires the XML node for this primary
			if(m_xmlNode == null) return false;
			
			//	Clear the secondaries collection
			Debug.Assert(m_xmlSecondaries != null);
			m_xmlSecondaries.Clear();
			this.Children = 0;
			
			//	Locate all the secondary nodes
			foreach(XmlNode O in m_xmlNode.ChildNodes)
			{
				if(String.Compare(O.Name, "secondary", true) != 0) continue;
				
				xmlSecondary = new CXmlSecondary(O);
				m_tmaxEventSource.Attach(xmlSecondary.EventSource);	
						
				if(xmlSecondary.SetProperties(O) == false) continue;

				//	Add to the collection
				m_xmlSecondaries.Add(xmlSecondary);
				
			}// foreach(XmlNode O in m_xmlNode.ChildNodes)
			
			this.Children = m_xmlSecondaries.Count;
			m_xmlSecondaries.Sort(true);
			
			return true;
			
		}// public bool GetSecondaries()
			
		#endregion Public Methods
		
		#region Properties
		
		//	XML node used to initialize this object
		public XmlNode Node
		{
			get{ return m_xmlNode; }
			set{ m_xmlNode = value; }
		}
		
		//	Collection of XML secondaries owned by the primary
		public CXmlSecondaries XmlSecondaries
		{
			get{ return m_xmlSecondaries; }
		}
		
		//	Unique MediaId assigned to the primary record in the database
		public string MediaId
		{
			get{ return m_strMediaId; }
			set{ m_strMediaId = value; }
		}
		
		//	Name assigned to the primary record in the database
		public string Name
		{
			get{ return m_strName; }
			set{ m_strName = value; }
		}
		
		//	Description assigned to the primary record in the database
		public string Description
		{
			get{ return m_strDescription; }
			set{ m_strDescription = value; }
		}
		
		//	Foreign barcode assigned to the primary record in the database
		public string ForeignBarcode
		{
			get{ return m_strForeignBarcode; }
			set{ m_strForeignBarcode = value; }
		}
		
		//	The number of secondary children associated with this primary
		public int Children
		{
			get{ return m_iChildren; }
			set{ m_iChildren = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlPrimary

	/// <summary>
	/// Objects of this class are used to manage a dynamic array of CXmlPrimary objects
	/// </summary>
	public class CXmlPrimaries : CTmaxSortedArrayList
	{
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlPrimaries() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="xmlPrimary">CXmlPrimary object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlPrimary Add(CXmlPrimary xmlPrimary)
		{
			try
			{
				// Use base class to perform actual collection operation
				base.Add(xmlPrimary as object);

				return xmlPrimary;
			}
			catch
			{
				return null;
			}
			
		}// Add(CXmlPrimary xmlPrimary)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlPrimary">The object to be removed</param>
		public void Remove(CXmlPrimary xmlPrimary)
		{
			try
			{
				// Use base class to process actual collection operation
				base.Remove(xmlPrimary as object);
			}
			catch
			{
			}
		}
		
		/// <returns>The object with the specified MediaId</returns>
		public CXmlPrimary Find(string strMediaId)
		{
			// Search for the object with the same MediaId
			foreach(CXmlPrimary O in this)
			{
				if(String.Compare(O.MediaId, strMediaId, true) == 0)
				{
					return O;
				}
			}
			return null;

		}//	Find(string MediaId)

		/// <summary>This method is called to clear the collection</summary>
		public override void Clear()
		{
			base.Clear();
		}

		/// <summary>This method is called to determine if the specified object exists in the collection</summary>
		/// <param name="xmlPrimary">The object to be located</param>
		/// <returns>true if found in the collection</returns>
		public bool Contains(CXmlPrimary xmlPrimary)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlPrimary as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlPrimary this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlPrimary);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlPrimary value)
		{
			// Find the 0 based index of the requested entry
			return base.IndexOf(value);
		}

		#endregion Public Methods
		
	}//	public class CXmlPrimaries
		
}// namespace FTI.Shared.Xml
