using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML TrialMax case descriptor</summary>
	public class CXmlCase : CXmlBase
	{
		#region Constants
		
		private const string XML_CASE_ELEMENT_NAME = "case";

		private const string XML_CASE_ATTRIBUTE_DATABASE_GUID = "databaseGUID";
		private const string XML_CASE_ATTRIBUTE_VERSION = "version";
		private const string XML_CASE_ATTRIBUTE_CASE_PATH = "casePath";
		private const string XML_CASE_ATTRIBUTE_NAME = "name";
		private const string XML_CASE_ATTRIBUTE_SHORT_NAME = "shortName";
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>This member is bounded to the TmaxCase property</summary>
		private CTmaxCase m_tmaxCase = null;	
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlCase() : base()
		{
		}

		/// <summary>Constructor</summary>
		public CXmlCase(CTmaxCase tmaxCase) : base()
		{
			this.TmaxCase = tmaxCase;
		}

		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source link to be copied</param>
		public CXmlCase(CXmlCase xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
				
			if(xmlSource != null)
				Copy(xmlSource);
		}
		
		/// <summary>Called to copy the properties of the source node</summary>
		public void Copy(CXmlCase xmlSource)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
					
			//	Copy the application wrapper
			this.TmaxCase.Copy(xmlSource.TmaxCase);
					
		}// public void Copy(CXmlCase xmlSource)

		/// <summary>This method is called to get the application case object</summary>
		/// <returns>The application object</returns>
		public CTmaxCase GetTmaxCase()
		{
			//	Do we need to allocate the object?
			if(m_tmaxCase == null)
				m_tmaxCase = new CTmaxCase();

			return m_tmaxCase;

		}// public CTmaxCase GetTmaxCase()
		
		/// <summary>This method will set the case properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML case properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the case properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_CASE_ATTRIBUTE_DATABASE_GUID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.UniqueId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_CASE_ATTRIBUTE_VERSION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Version = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_CASE_ATTRIBUTE_SHORT_NAME, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.ShortName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_CASE_ATTRIBUTE_NAME, "");
				if((strAttribute != null) && (strAttribute.Length > 0))
					this.Name = strAttribute;

				//	If the Name is not in the file this could be an old version (before 6.3.0)
				if(this.Name.Length == 0)
				{
					//	The case path should be in the file if this is an old file
					strAttribute = xpNavigator.GetAttribute(XML_CASE_ATTRIBUTE_CASE_PATH,"");
					if((strAttribute != null) && (strAttribute.Length > 0))
					{
						//	Strip the filename
						this.Name = System.IO.Path.GetDirectoryName(strAttribute);
				
						//	Set the name to match the folder name
						if(this.Name.Length > 0)
							this.Name = System.IO.Path.GetFileName(this.Name);

					}// if((strAttribute != null) && (strAttribute.Length > 0))

				}// if(this.Name.Length == 0)

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML case properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
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
				strElementName = XML_CASE_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_CASE_ATTRIBUTE_NAME, this.Name) == false)
						break;
					
					if(AddAttribute(xmlElement, XML_CASE_ATTRIBUTE_DATABASE_GUID, this.UniqueId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_CASE_ATTRIBUTE_VERSION, this.Version) == false)
						break;

					if(AddAttribute(xmlElement, XML_CASE_ATTRIBUTE_SHORT_NAME, this.ShortName) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Properties

		//	The application wrapper object for the case descriptor
		public CTmaxCase TmaxCase
		{
			get { return GetTmaxCase(); }
			set { m_tmaxCase = value; }
		}

		//	The Name assigned to the case
		public string Name
		{
			get { return this.TmaxCase.Name; }
			set { this.TmaxCase.Name = value; }
		}

		//	The short name assigned to the case
		public string ShortName
		{
			get { return this.TmaxCase.ShortName; }
			set { this.TmaxCase.ShortName = value; }
		}

		//	GUID of master database
		public string UniqueId
		{
			get{ return this.TmaxCase.UniqueId; }
			set { this.TmaxCase.UniqueId = value; }
		}
		
		//	Trialmax version identifier
		public string Version
		{
			get { return this.TmaxCase.Version; }
			set { this.TmaxCase.Version = value; }
		}
	
		#endregion Properties
		
	}// public class CXmlCase

}// namespace FTI.Shared.Xml
