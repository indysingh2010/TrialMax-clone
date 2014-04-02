using System;
using System.Collections;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class is used to create and manage an Xml file</summary>
	public class CXmlIni
	{
		#region Constants
		
		private const string XMLINI_ROOT_NODE			= "trialMax";
		private const string XMLINI_SECTIONS_NODE		= "Sections";
		private const string XMLINI_DEFAULT_ATTRIBUTE	= "Value";
		
		private const int ERROR_ADD_ATTRIBUTE_EX		= 0;
		private const int ERROR_ADD_ELEMENT_EX			= 1;
		private const int ERROR_CREATE_FILE_EX			= 2;
		private const int ERROR_OPEN_FILE_EX			= 3;
		private const int ERROR_SAVE_FILE_EX			= 4;
		private const int ERROR_WRITE_FILE_EX			= 5;
		private const int ERROR_NO_SECTIONS_NODE		= 6;
		private const int ERROR_NO_ROOT_NODE			= 7;
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		protected CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member bound to EventSource property</summary>
		protected CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Local member accessed by the XMLDocument property</summary>
		protected XmlDocument m_xmlDocument = null;			
		
		/// <summary>Local member accessed by the XMLRoot property</summary>
		protected XmlNode m_xmlRoot = null;			
		
		/// <summary>Local member accessed by the XMLSections property</summary>
		protected XmlNode m_xmlSections = null;			
		
		/// <summary>Local member accessed by the XMLSection property</summary>
		protected XmlNode m_xmlSection = null;			
		
		/// <summary>Local member accessed by the FileSpec property</summary>
		protected string m_strFileSpec = "";			
		
		/// <summary>Local member accessed by the Section property</summary>
		protected string m_strSection = "";			
		
		/// <summary>Local member accessed by the XMLComments property</summary>
		protected ArrayList m_xmlComments = new ArrayList();		
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlIni()
		{
			//	Populate the error builder
			SetErrorStrings();
		
		}// public CXmlIni()
		
		/// <summary>This method is called to close the file</summary>
		public void Close()
		{
			m_xmlSection = null;
			m_xmlSections = null;
			m_xmlRoot = null;
			m_xmlDocument = null;
			m_strFileSpec = "";
			m_strSection = "";
		
		}// public void Close()
			
		/// <summary>This method is called to open the file and load the Xml document</summary>
		/// <param name="strFileSpec">The fully qualified path to the desired file</param>
		/// <param name="strSection">Optional name of section to locate after opening the file</param>
		/// <returns>true if successful</returns>
		public bool Open(string strFileSpec, string strSection)
		{
			bool bSuccessful = false;
			
			//	Close the existing document
			Close();
			
			//	Do we need to create the xml file?
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				//	Create the file
				if(Create(strFileSpec) == false)
					return false;
			}
			
			try
			{
				while(bSuccessful == false)
				{
					//	Save the file specification
					m_strFileSpec = strFileSpec;
					m_strFileSpec.ToLower();

					//	Create the document object
					m_xmlDocument = new XmlDocument();

					//	Load and parse the XML
					m_xmlDocument.Load(strFileSpec);
					
					//	Get the root node for the document
					if((m_xmlRoot = m_xmlDocument.DocumentElement) == null)
					{
						m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_ROOT_NODE, strFileSpec));
						break;
					}
					
					//	Get the Sections node 
					if(GetXmlSections(true) == false)
					{
						m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_SECTIONS_NODE, strFileSpec));
						break;
					}
					
					//	Should we initialize the section?
					if((strSection != null) && (strSection.Length > 0))
					{
						if(SetSection(strSection) == false)
						{
							Close();
							return false;
						}
						
					}
					
					//	It's all good
					bSuccessful = true;
					
				}// while(bSuccessful == false)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_FILE_EX, m_strFileSpec), Ex);
			}
			
			//	Clean up if not successful
			if(bSuccessful == false)
				Close();
				
			return bSuccessful;

		}//	public bool Open(string strFileSpec, string strSection)
		
		/// <summary>This method is called to open the file and load the Xml document</summary>
		/// <param name="strFileSpec">The fully qualified path to the desired file</param>
		/// <returns>true if successful</returns>
		public bool Open(string strFileSpec)
		{
			return Open(strFileSpec, null);
		}
		
		/// <summary>This method will get the specified key node from the active section</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being set</param>
		///	<param name="objValue">The value to be assigned to the attribute</param>
		/// <returns>true if successful</returns>
		public virtual bool Write(string strKey, string strAttribute, object objValue)
		{
			XmlElement		xmlKey = null;
			XmlAttribute	xmlAttribute = null;
			
			if(m_xmlSection == null) return false;
			if(m_xmlSection.ChildNodes == null) return false;
			
			try
			{
				//	Create the key element if it does not already exist
				if((xmlKey = GetKeyElement(strKey)) == null)
				{
					return (AddKey(strKey, strAttribute, objValue) != null);
				}
				else
				{
					//	Get the specified attribute
					if(xmlKey.Attributes != null)
						xmlAttribute = xmlKey.Attributes[strAttribute];
						
					//	Do we need to add this attribute?
					if(xmlAttribute == null)
					{
						return AddAttribute(xmlKey, strAttribute, objValue);
					}
					else
					{
						xmlAttribute.Value = objValue.ToString();
						return true;
					}
					
				}
					
			}
			catch
			{
				return false;
			}
			
		}// protected virtual bool Write(string strKey, string strAttribute, object objValue)
			
		/// <summary>This method will get the specified key node from the active section</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="objValue">The value to be assigned to the Value attribute</param>
		/// <returns>true if successful</returns>
		public virtual bool Write(string strKey, object objValue)
		{
			return Write(strKey, XMLINI_DEFAULT_ATTRIBUTE, objValue);	
			
		}// protected virtual bool Write(string strKey, object objValue)
			
		/// <summary>This method will read the specified key attribute from the active section</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="objValue">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public virtual string Read(string strKey, string strAttribute, object objDefault)
		{
			XmlElement		xmlKey = null;
			XmlAttribute	xmlAttribute = null;
			string			strDefault = (objDefault != null) ? objDefault.ToString() : "";
			
			if(m_xmlSection == null) return strDefault;
			if(m_xmlSection.ChildNodes == null) return strDefault;
			
			try
			{
				//	Get the requested key
				if((xmlKey = GetKeyElement(strKey)) != null)
				{
					//	Get the requested attribute
					if(xmlKey.Attributes != null)
					{
						if((xmlAttribute = xmlKey.Attributes[strAttribute]) != null)
						{
							return xmlAttribute.Value;
						}
						
					}
					
				}
					
			}
			catch
			{
			}
			
			//	Must not have found the specified attribute
			return strDefault;
			
		}// public virtual string Read(string strKey, string strAttribute, object objDefault)
			
		/// <summary>This method will read the default key attribute from the active section</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="objValue">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public virtual string Read(string strKey, object objDefault)
		{
			return Read(strKey, XMLINI_DEFAULT_ATTRIBUTE, objDefault);
			
		}// protected virtual bool Read(string strKey, object objDefault)
			
		/// <summary>This method will read the default key attribute from the active section</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the specified key if found</returns>
		public virtual string Read(string strKey)
		{
			return Read(strKey, XMLINI_DEFAULT_ATTRIBUTE, "");
			
		}// protected virtual bool Read(string strKey)
			
		/// <summary>This method will set the current section</summary>
		/// <param name="strSection">Name of the new section</param>
		/// <param name="bCreate">True to create it if it doesn't exist</param>
		/// <param name="bFlush">true to flush the existing contents</param>
		/// <returns></returns>
		public virtual bool SetSection(string strSection, bool bCreate, bool bFlush)
		{
			if(m_xmlSections == null) return false;
			if(m_xmlSections.ChildNodes == null) return false;

			//	See if the section already exists
			foreach(XmlNode xmlNode in m_xmlSections.ChildNodes)
			{				
				if(String.Compare(GetSectionName(xmlNode), strSection, true) == 0)
				{
					m_xmlSection = xmlNode;
					m_strSection = strSection;
							
					//	Should we flush the child nodes?
					if(bFlush == true)
					{
						while(m_xmlSection.ChildNodes.Count > 0)
							m_xmlSection.RemoveChild(m_xmlSection.ChildNodes[0]);
					}
							
					return true;
				}
				
			}// foreach(XmlNode xmlNode in m_xmlSections.ChildNodes)

			//	We have to create a new section node
			if(bCreate == true)
			{
				if((m_xmlSection = AddSection(strSection)) != null)
				{
					m_strSection = strSection;
					return true;
				}
			
			}// if(bCreate == true)
		
			//	Unable to get the xml section node
			return false;
		
		}// public virtual bool SetSection(string strSection, bool bCreate)
		
		/// <summary>This method will set the current section</summary>
		/// <param name="strSection">Name of the new section</param>
		/// <param name="bCreate">True to create it if it doesn't exist</param>
		/// <returns></returns>
		public virtual bool SetSection(string strSection, bool bCreate)
		{
			return SetSection(strSection, bCreate, false);
		}

		/// <summary>This method will set the current section</summary>
		/// <param name="strSection">Name of the new section</param>
		/// <returns></returns>
		public virtual bool SetSection(string strSection)
		{
			return SetSection(strSection, true);
		}

		/// <summary>This method will delete the specified section</summary>
		/// <param name="strSection">Name of the section</param>
		public virtual bool DeleteSection(string strSection)
		{
			XmlNode	xmlSection = null;
			
			if (m_xmlSections == null)
				return false;
			if (m_xmlSections.ChildNodes == null)
				return false;

			//	See if the section exists
			foreach(XmlNode xmlNode in m_xmlSections.ChildNodes)
			{
				if(String.Compare(GetSectionName(xmlNode), strSection, true) == 0)
				{
					xmlSection = xmlNode;
					break;
				}

			}// foreach(XmlNode xmlNode in m_xmlSections.ChildNodes)

			if(xmlSection != null)
			{
				//	Are we deleting the active section?
				if ((m_xmlSection != null) && (String.Compare(m_strSection, strSection, true) == 0))
				{
					m_xmlSection = null;
					m_strSection = "";
				}

				m_xmlSections.RemoveChild(xmlSection);
			}
			
			return (xmlSection != null);

		}// public virtual bool DeleteSection(string strSection)

		/// <summary>This method will get the name of the section bound to the specified node</summary>
		/// <param name="xmlSection">The XML node containing the section information</param>
		/// <returns>The name of the section</returns>
		public virtual string GetSectionName(XmlNode xmlSection)
		{
			XmlAttribute xmlName = null;
			string		 strName = "";
			
			Debug.Assert(xmlSection != null);

			if((xmlSection != null) && (xmlSection.Attributes != null))
			{
				//	Get the attribute containing the name
				if((xmlName = xmlSection.Attributes["Name"]) != null)
					strName = xmlName.Value;
			
			}// if((xmlSection != null) && (xmlSection.Attributes != null))
		
			return strName;
		
		}// public virtual string GetSectionName(XmlNode xmlSection)
		
		/// <summary>This method will read the specified key attribute as a short integer</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public short ReadShort(string strKey, string strAttribute, short sDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return System.Convert.ToInt16(strValue);
				}
				catch
				{
					return sDefault;
				}
				
			}
			else
			{
				return sDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as a short integer</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public short ReadShort(string strKey, short sDefault)
		{
			return ReadShort(strKey, XMLINI_DEFAULT_ATTRIBUTE, sDefault);
		}
		
		/// <summary>This method will read the default attribute as a short integer</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public short ReadShort(string strKey)
		{
			return ReadShort(strKey, XMLINI_DEFAULT_ATTRIBUTE, 0);
		}
		
		/// <summary>This method will read the specified key attribute as an integer integer</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public int ReadInteger(string strKey, string strAttribute, int iDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return System.Convert.ToInt32(strValue);
				}
				catch
				{
					return iDefault;
				}
				
			}
			else
			{
				return iDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as an integer integer</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public int ReadInteger(string strKey, int iDefault)
		{
			return ReadInteger(strKey, XMLINI_DEFAULT_ATTRIBUTE, iDefault);
		}
		
		/// <summary>This method will read the default attribute as an integer integer</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public int ReadInteger(string strKey)
		{
			return ReadInteger(strKey, XMLINI_DEFAULT_ATTRIBUTE, 0);
		}
		
		/// <summary>This method will read the specified key attribute as an long</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public long ReadLong(string strKey, string strAttribute, long lDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return System.Convert.ToInt64(strValue);
				}
				catch
				{
					return lDefault;
				}
				
			}
			else
			{
				return lDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as an long integer</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public long ReadLong(string strKey, long lDefault)
		{
			return ReadLong(strKey, XMLINI_DEFAULT_ATTRIBUTE, lDefault);
		}
		
		/// <summary>This method will read the default attribute as a long integer</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public long ReadLong(string strKey)
		{
			return ReadLong(strKey, XMLINI_DEFAULT_ATTRIBUTE, 0);
		}
		
		/// <summary>This method will read the specified key attribute as as a single precision floating point value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public float ReadSingle(string strKey, string strAttribute, float fDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return System.Convert.ToSingle(strValue);
				}
				catch
				{
					return fDefault;
				}
				
			}
			else
			{
				return fDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as as a single precision floating point value</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="sDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public float ReadSingle(string strKey, float fDefault)
		{
			return ReadSingle(strKey, XMLINI_DEFAULT_ATTRIBUTE, fDefault);
		}
		
		/// <summary>This method will read the default attribute as a single precision floating point value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public float ReadSingle(string strKey)
		{
			return ReadSingle(strKey, XMLINI_DEFAULT_ATTRIBUTE, 0);
		}
		
		/// <summary>This method will read the specified key attribute as as a double precision doubleing point value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="dDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public double ReadDouble(string strKey, string strAttribute, double dDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return System.Convert.ToDouble(strValue);
				}
				catch
				{
					return dDefault;
				}
				
			}
			else
			{
				return dDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as as a double precision doubleing point value</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="dDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public double ReadDouble(string strKey, double dDefault)
		{
			return ReadDouble(strKey, XMLINI_DEFAULT_ATTRIBUTE, dDefault);
		}
		
		/// <summary>This method will read the default attribute as as a double precision doubleing point value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public double ReadDouble(string strKey)
		{
			return ReadDouble(strKey, XMLINI_DEFAULT_ATTRIBUTE, 0);
		}
		
		/// <summary>This method will read the specified key attribute as as a boolean value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="bDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public bool ReadBool(string strKey, string strAttribute, bool bDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return CTmaxToolbox.StringToBool(strValue);
				}
				catch
				{
					return bDefault;
				}
				
			}
			else
			{
				return bDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as as a boolean value</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="bDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public bool ReadBool(string strKey, bool bDefault)
		{
			return ReadBool(strKey, XMLINI_DEFAULT_ATTRIBUTE, bDefault);
		}
		
		/// <summary>This method will read the default attribute as as a boolean value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public bool ReadBool(string strKey)
		{
			return ReadBool(strKey, XMLINI_DEFAULT_ATTRIBUTE, false);
		}
		
		/// <summary>This method will read the specified key attribute as as a date/time value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="dtDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public DateTime ReadDateTime(string strKey, string strAttribute, DateTime dtDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return System.Convert.ToDateTime(strValue);
				}
				catch
				{
					return dtDefault;
				}
				
			}
			else
			{
				return dtDefault;
			}	
			
		}
		
		/// <summary>This method will read the default attribute as as a date/time value</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="dtDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the default key attribute if found</returns>
		public DateTime ReadDateTime(string strKey, DateTime dtDefault)
		{
			return ReadDateTime(strKey, XMLINI_DEFAULT_ATTRIBUTE, dtDefault);
		}
		
		/// <summary>This method will read the default attribute as as a date/time value</summary>
		/// <param name="strKey">The desired key element</param>
		/// <returns>The value of the default key attribute if found</returns>
		public DateTime ReadDateTime(string strKey)
		{
			return ReadDateTime(strKey, XMLINI_DEFAULT_ATTRIBUTE, System.DateTime.Now);
		}
		
		/// <summary>This method will read the specified key attribute as an enumerated object</summary>
		/// <param name="strKey">The desired key element</param>
		/// <param name="strAttribute">The name of the key attribute being read</param>
		///	<param name="eDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public Enum ReadEnum(string strKey, string strAttribute, Enum eDefault)
		{
			string strValue = Read(strKey, strAttribute, "");
			
			if(strValue.Length > 0)
			{
				try
				{
					return (Enum)Enum.Parse(eDefault.GetType(), strValue, true);
				}
				catch
				{
					return eDefault;
				}
				
			}
			else
			{
				return eDefault;
			}	
			
		}
		
		/// <summary>This method will read the default key attribute as an enumerated object</summary>
		/// <param name="strKey">The desired key element</param>
		///	<param name="eDefault">The default value returned if the attribute is not found</param>
		/// <returns>The value of the specified key if found</returns>
		public Enum ReadEnum(string strKey, Enum eDefault)
		{
			return ReadEnum(strKey, XMLINI_DEFAULT_ATTRIBUTE, eDefault);
		}
		
		/// <summary>This method will add a new section</summary>
		/// <param name="strName">The section name</param>
		/// <returns>The new element node if successful</returns>
		public XmlElement AddSection(string strName)
		{
			XmlElement xmlSection;
			
			//	Make sure we have the required objects
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlSections != null);
			if(m_xmlDocument == null) return null;
			if(m_xmlSections == null) return null;
			
			try
			{
				if((xmlSection = m_xmlDocument.CreateElement("sect", "Section", "fticonsulting.com/xsd/ini-section")) != null)
				{
					if(AddAttribute(xmlSection, "Name", strName) == true)
					{
						m_xmlSections.AppendChild(xmlSection);
						return xmlSection;
					}	
				}
			}
			catch
			{
			}
			
			//	Must have been an error
			return null;
		}
	
		/// <summary>This method will add a key to the active section</summary>
		/// <param name="strName">The name of the key element to be added</param>
		/// <param name="strAttribute">The name of the initial attribute assigned to the key</param>
		/// <param name="objValue">The value of the attribute assigned to the key</param>
		/// <returns>The new element node if successful</returns>
		public XmlElement AddKey(string strName, string strAttribute, object objValue)
		{
			XmlElement xmlKey;
			
			//	Make sure we have the required objects
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlSections != null);
			Debug.Assert(m_xmlSection != null);
			if(m_xmlDocument == null) return null;
			if(m_xmlSections == null) return null;
			if(m_xmlSection == null) return null;
			
			try
			{
				if((xmlKey = m_xmlDocument.CreateElement(strName)) != null)
				{
					if(AddAttribute(xmlKey, strAttribute, objValue) == true)
					{
						m_xmlSection.AppendChild(xmlKey);
						return xmlKey;
					}	
				}
			}
			catch
			{
			}
			
			//	Must have been an error
			return null;
		}
	
		/// <summary>
		/// This method will add a child element to the specified node
		/// </summary>
		/// <param name="xmlParent">The node to which the new child is added</param>
		/// <param name="strName">The element name</param>
		/// <param name="objValue">The element value</param>
		/// <returns>The new element node if successful</returns>
		public XmlElement AddElement(XmlNode xmlParent, string strName, object objValue)
		{
			return AddElement(xmlParent, null, strName, null, objValue);
		}
	
		/// <summary>
		/// This method will add a child element to the specified node
		/// </summary>
		/// <param name="xmlParent">The node to which the new child is added</param>
		/// <param name="strPrefix">The node's prefix</param>
		/// <param name="strName">The element name</param>
		///	<param name="strNamespace">The namespace associated with the element</param>
		/// <param name="objValue">The element value</param>
		/// <returns>The new element node if successful</returns>
		public XmlElement AddElement(XmlNode xmlParent, string strPrefix, string strName, string strNamespace, object objValue)
		{
			XmlElement xmlChild = null;
		
			//	We should have the document object
			Debug.Assert(m_xmlDocument != null);
			if(m_xmlDocument == null) return null;
			
			try
			{
				//	If you supply a prefix you must also have a namespace
				if((strPrefix != null) && (strPrefix.Length > 0) && (strNamespace != null) && (strNamespace.Length > 0))
					xmlChild = m_xmlDocument.CreateElement(strPrefix, strName, strNamespace);
				else
					xmlChild = m_xmlDocument.CreateElement(strName);
					
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
		}
	
		/// <summary>This method will add an attribute to the specified node</summary>
		/// <param name="xmlElement">Element node associated with the attribute</param>
		/// <param name="strName">Name of the attribute</param>
		/// <param name="objValue">Value of the attribute</param>
		/// <returns>true if successful</returns>
		public bool AddAttribute(XmlElement xmlElement, string strName, object objValue)
		{
			try
			{
				if(objValue != null)
				{
					xmlElement.SetAttribute(strName, objValue.ToString());
					return true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "AddAttribute", m_tmaxErrorBuilder.Message(ERROR_ADD_ATTRIBUTE_EX, strName, objValue != null ? objValue.ToString() : "null"), Ex);
			}
			
			return false;
		}
					
		/// <summary>This method is called to save the current document to file</summary>
		/// <returns>true if successful</returns>
		public virtual bool Save()
		{
			XmlTextWriter xmlWriter = null;

			//	Do we have a valid document?
			if(m_xmlDocument == null) return false;
			
			try
			{
				if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
				{
					xmlWriter.Formatting = Formatting.Indented;
					xmlWriter.Indentation = 4;
					
					m_xmlDocument.Save(xmlWriter);

                    xmlWriter.Close();
					return true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
                MessageBox.Show(string.Format("FileSpec:{0} \n xmlWriter: {1} \n {2}", m_strFileSpec, xmlWriter != null,Ex.Message),"Error");
					
			}
			
			return false;
		
		}//	Save()
		
		/// <summary>This method will get the specified key node from the active section</summary>
		public virtual XmlElement GetKeyElement(string strKey)
		{
			if(m_xmlSection == null) return null;
			if(m_xmlSection.ChildNodes == null) return null;
			
			//	See if the section already exists
			foreach(XmlElement xmlElement in m_xmlSection.ChildNodes)
			{
				if(xmlElement.LocalName != null)
				{
					if(String.Compare(xmlElement.LocalName, strKey, true) == 0)
					{
						return xmlElement;
					}
				}
				
			}
			
			//	Unable to locate the specified node
			return null;
		}
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to create the XML file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <returns>true if successful</returns>
		protected virtual bool Create(string strFileSpec)
		{
			XmlTextWriter xmlWriter = null;
			
			try
			{
				//	Create an Xml text writer using the current file specification
				if((xmlWriter = new XmlTextWriter(strFileSpec, null)) != null)
				{
					xmlWriter.WriteStartDocument();
					
					if((m_xmlComments != null) && (m_xmlComments.Count > 0))
					{
						foreach(object O in m_xmlComments)
						{
							xmlWriter.WriteComment(O.ToString());
						}
					}
					
					xmlWriter.WriteStartElement(XMLINI_ROOT_NODE);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
					xmlWriter.Close();
					
					return true;

				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Create", m_tmaxErrorBuilder.Message(ERROR_CREATE_FILE_EX, m_strFileSpec), Ex);
			}
			
			return false;
		
		}//	Create()
		
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
			aStrings.Add("An exception was raised while attempting to create the XML file: filename = %1");
			aStrings.Add("An exception was raised while attempting to open the XML file: filename = %1");
			aStrings.Add("An exception was raised while attempting to save the XML file: filename = %1");
			aStrings.Add("An exception was raised while attempting to write an object to the XML file: filename = %1");
			aStrings.Add("%1 is not a valid XML INI file. It does not contain a root sections node.");
			aStrings.Add("%1 is not a valid XML INI file. It does not contain a root node.");
		}
		
		/// <summary>This method will get the XML Sections node</summary>
		/// <param name="bCreate">True to create the node if it does not exist</param>
		/// <returns>True if found</returns>
		public bool GetXmlSections(bool bCreate)
		{
			//	Must have a root node
			Debug.Assert(m_xmlRoot != null);
			if(m_xmlRoot == null) return false;
			
			m_xmlSections = null;
			
			//	In early versions of this class the Sections node was the root
			if(String.Compare(m_xmlRoot.Name, XMLINI_SECTIONS_NODE, true) == 0)
			{
				m_xmlSections = m_xmlRoot;
			}
			else
			{
				//	Assume the Sections node is a child of the root
				foreach(XmlNode O in m_xmlRoot.ChildNodes)
				{
					if(String.Compare(O.Name, XMLINI_SECTIONS_NODE, true) == 0)
					{
						m_xmlSections = O;
						break;
					}
					
				}// foreach(XmlNode O in m_xmlRoot.ChildNodes)
				
			}// if(String.Compare(m_xmlRoot.Name, XML_SECTIONS_NODE, true) == 0)
			
			//	Should we create the Sections node?
			if((m_xmlSections == null) && (bCreate == true))
				return AddXmlSections();
			else
				return (m_xmlSections != null);
		
		}// public bool GetXmlSections(bool bCreate)
		
		/// <summary>This method will add the XML Sections node to the root of the active document</summary>
		/// <returns>True if successful</returns>
		public bool AddXmlSections()
		{
			XmlElement xmlSections = null;
			
			//	Must have a root node
			Debug.Assert(m_xmlRoot != null);
			if(m_xmlRoot == null) return false;
			
			try
			{
				if((xmlSections = m_xmlDocument.CreateElement(XMLINI_SECTIONS_NODE)) != null)
				{
					m_xmlRoot.AppendChild(xmlSections);
				}
			
			}
			catch
			{
				xmlSections = null;
			}
			
			m_xmlSections = xmlSections;
			return (m_xmlSections != null);
		
		}// public bool GetXmlSections(bool bCreate)
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Event source interface for error and diagnostic events</summary>
		public CTmaxEventSource EventSource
		{
			get	{ return m_tmaxEventSource; }
		}
		
		/// <summary>XML document object associated with the file</summary>
		public XmlDocument XMLDocument
		{
			get { return m_xmlDocument; }
		}

		/// <summary>XML root node</summary>
		public XmlNode XMLRoot
		{
			get { return m_xmlRoot;  }
		}

		/// <summary>XML node containing the sections collection</summary>
		public XmlNode XMLSections
		{
			get { return m_xmlSections;  }
			set	{ m_xmlSections = value; }
		}

		/// <summary>XML node associated with the current section</summary>
		public XmlNode XMLSection
		{
			get { return m_xmlSection; }
		}

		/// <summary>XML comments added when the file is created</summary>
		public ArrayList XMLComments
		{
			get	{ return m_xmlComments; }
		}

		/// <summary>Flag to indicate if the file is open</summary>
		public bool IsOpen
		{
			get { return (m_xmlDocument != null); }
		}

		/// <summary>Full file specification for the log file</summary>
		public string FileSpec
		{
			get { return m_strFileSpec; }
		}

		/// <summary>Name of the active section</summary>
		public string Section
		{
			get { return m_strSection; }
		}

		#endregion Properties
		
	}//	class CXmlIni
	
}// namespace FTI.Shared.Xml
