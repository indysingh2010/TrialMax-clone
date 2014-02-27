using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition designation</summary>
	public class CXmlProductUpdate : CXmlFile
	{
		#region Constants
		
		public const string XML_PRODUCT_UPDATE_ROOT_NAME = "trialMax";
		public const string XML_PRODUCT_UPDATE_ELEMENT_NAME = "productUpdate";

		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_SOURCE       = "source";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_CREATED_BY   = "createdBy";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_CREATED_ON   = "createdOn";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_MODIFIED_BY  = "modifiedBy";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_MODIFIED_ON  = "modifiedOn";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_RETRIEVED_BY = "retrievedBy";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_RETRIEVED_ON = "retrievedOn";
		public const string XML_PRODUCT_UPDATE_ATTRIBUTE_APP_FILENAME = "appFilename";

		protected const int ERROR_OPEN_PRODUCT_UPDATE_EX	= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_PRODUCT_UPDATE_NODE	= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX			= (LAST_XML_FILE_ERROR + 3);
		protected const int	ERROR_FAST_FILL_EX				= (LAST_XML_FILE_ERROR + 4);
		protected const int	ERROR_GET_UPDATES_EX			= (LAST_XML_FILE_ERROR + 5);
		
		#endregion Constants
		
		#region Protected Members

		/// <summary>Local member bound to Updates property</summary>
		protected CXmlUpdates m_xmlUpdates = new CXmlUpdates();

		/// <summary>This member is bounded to the Source property</summary>
		protected string m_strSource = "";		
		
		/// <summary>This member is bounded to the CreatedBy property</summary>
		protected string m_strCreatedBy = "";
		
		/// <summary>This member is bounded to the ModifiedOn property</summary>
		protected string m_strCreatedOn = "";
		
		/// <summary>This member is bounded to the CreatedOn property</summary>
		protected string m_strModifiedOn = "";
		
		/// <summary>This member is bounded to the ModifiedBy property</summary>
		protected string m_strModifiedBy = "";
		
		/// <summary>This member is bounded to the RetrievedBy property</summary>
		protected string m_strRetrievedBy = "";
		
		/// <summary>This member is bounded to the RetrievedOn property</summary>
		protected string m_strRetrievedOn = "";
		
		/// <summary>This member is bounded to the AppFilename property</summary>
		protected string m_strAppFilename = "";
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlProductUpdate() : base()
		{
			m_strRoot = XML_PRODUCT_UPDATE_ROOT_NAME;
			Extension = GetDefaultExtension();
			
			m_xmlUpdates.Comparer = new CXmlBaseSorter();
			m_xmlUpdates.KeepSorted = false;
			
			m_tmaxEventSource.Name = "XML Product Update Events";
			m_tmaxEventSource.Attach(m_xmlUpdates.EventSource);

		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlSource">Source node to be copied</param>
		public CXmlProductUpdate(CXmlProductUpdate xmlSource) : base()
		{
			Debug.Assert(xmlSource != null);
					
			if(xmlSource != null)
				Copy(xmlSource);
		
			m_tmaxEventSource.Name = "XML Product Update Events";
			m_tmaxEventSource.Attach(m_xmlUpdates.EventSource);

		}// public CXmlProductUpdate(CXmlProductUpdate xmlSource) : base()
		
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		public void Copy(CXmlProductUpdate xmlSource)
		{
			Copy(xmlSource, true);
		}
			
		/// <summary>Called to copy the properties of the source node</summary>
		///	<param name="xmlSource">the source node to be copied</param>
		/// <param name="bUpdates">true to copy the updates</param>
		public void Copy(CXmlProductUpdate xmlSource, bool bUpdates)
		{
			//	Copy the base class members
			base.Copy(xmlSource as CXmlBase);
							
			this.Source      = xmlSource.Source;
			this.CreatedBy   = xmlSource.CreatedBy;
			this.CreatedOn   = xmlSource.CreatedOn;
			this.ModifiedBy  = xmlSource.ModifiedBy;
			this.ModifiedOn  = xmlSource.ModifiedOn;
			this.RetrievedBy = xmlSource.RetrievedBy;
			this.RetrievedOn = xmlSource.RetrievedOn;
			this.AppFilename = xmlSource.AppFilename;
					
			//	Should we copy the update nodes?
			if(bUpdates == true)
			{
				m_xmlUpdates.Clear();
								
				if(xmlSource.Updates != null)
				{
					foreach(CXmlUpdate O in xmlSource.Updates)
						m_xmlUpdates.Add(new CXmlUpdate(O));
				}
						
			}
					
		}// public void Copy(CXmlUpdate xmlSource)
		
		/// <summary>This method is called to get the default file extension</summary>
		/// <returns>The default extension</returns>
		static public string GetDefaultExtension()
		{
			return "xml";
		}
		
		/// <summary>Called to clear the designation's child collections</summary>
		public void Clear()
		{
			if(m_xmlUpdates != null)
			{
				m_xmlUpdates.Clear();
				m_xmlUpdates = null;
			}
			
		}// public void Clear()
		
		/// <summary>This method is called to close the XML file associated with the deposition</summary>
		/// <param name="bClear">true to clear the local collections</param>
		public void Close(bool bClear)
		{
			//	Do the base class processing
			base.Close();
			
			//	Should we clear the collections?
			if(bClear == true)
				Clear();
		}
			
		/// <summary>This method is called to get open the specified file</summary>
		/// <param name="bCreate">true to create the file if it doesn't exist</param>
		/// <returns>true if successful</returns>
		public override bool Open(bool bCreate)
		{
			XmlNode xmlNode = null;
			bool	bSuccessful = false;
		
			//	Do the base class processing first
			if(base.Open(bCreate) == false) return false;
			
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlRoot != null);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Get the designation descriptor node
					if((xmlNode = m_xmlRoot.SelectSingleNode(XML_PRODUCT_UPDATE_ELEMENT_NAME)) == null)
					{
						//	Is this a new file?
						if(bCreate == true)
						{
							//	We don't expect the designation node to 
							//	appear in a new file
							bSuccessful = true;
						}
						else
						{
							m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_PRODUCT_UPDATE_NODE, m_strFileSpec));
						}
						break;
					}
					
					//	Get the properties
					if(SetProperties(xmlNode) == false)
						break;
						
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_PRODUCT_UPDATE_EX, m_strFileSpec), Ex);
			}
			
			if(bSuccessful == false)
			{
				Close(true);
				return false;
			}
			else
			{
				return true;
			}
			
		}//	Open(bool bCreate)
		
		/// <summary>This method is called to use an XPath document to intialize the object and
		///          its collections. The XPath document is closed when this method returns.
		/// </summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="bUpdates">true to fill the updates collection</param>
		/// <returns></returns>
		public bool FastFill(string strFileSpec, bool bUpdates)
		{
			XPathDocument		xpDocument  = null;
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;

			//	Make sure the file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, strFileSpec));
				return false;
			}
			
			try
			{
				//	Load the file
				xpDocument = new System.Xml.XPath.XPathDocument(strFileSpec);
				
				//	Get the deposition properties
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select("trialMax/productUpdate")) == null) return false;
				if(xpIterator.Count > 0)
				{
					xpIterator.MoveNext();
					SetProperties(xpIterator.Current);
				}
				else
				{
					m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_PRODUCT_UPDATE_NODE, strFileSpec));
					return false;
				}
				
				//	Update the base class file properties
				//
				//	NOTE:	This MUST be done in order to do subsequent calls
				//			to Save()
				SetFileProps(strFileSpec);
				
				//	Get the updates
				if(bUpdates == true)
					GetUpdates(xpDocument);

				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, strFileSpec), Ex);
			}
			
			//	Must have been a problem
			return false;
		}
		
		/// <summary>This method will set the product update properties using the specified node</summary>
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
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML designation properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the product update properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_SOURCE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSource = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_CREATED_BY,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCreatedBy = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_CREATED_ON,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCreatedOn = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_MODIFIED_BY,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strModifiedBy = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_MODIFIED_ON,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strModifiedOn = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_RETRIEVED_BY,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strRetrievedBy = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_RETRIEVED_ON,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strRetrievedOn = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_PRODUCT_UPDATE_ATTRIBUTE_APP_FILENAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strAppFilename = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", "An exception was raised while trying to set the XML product update properties", Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method is called to populate the updates collection</summary>
		/// <param name="xpDocument">The XPath document containing the update nodes</param>
		/// <returns>true if successful</returns>
		public bool GetUpdates(XPathDocument xpDocument)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			CXmlUpdate			xmlUpdate = null;
			
			try
			{
				//	Clear the collection
				m_xmlUpdates.Clear();
					
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select("trialMax/productUpdate/tmaxUpdate")) == null) return false;
			
				//	Add an object for each node
				while(xpIterator.MoveNext() == true)
				{
					xmlUpdate = new CXmlUpdate();
					xmlUpdate.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					xmlUpdate.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
					
					if(xmlUpdate.SetProperties(xpIterator.Current) == true)
						m_xmlUpdates.Add(xmlUpdate);
						
				}// while(xpIterator.MoveNext() == true)

				//	Make sure everything is properly sorted
				m_xmlUpdates.Sort(true);
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetUpdates", "An exception was raised while attempting to populate the list of updates", Ex);
				return false;
			}
			
		}// public bool GetUpdates(XPathDocument xpDocument)
		
		/// <summary>This method is called to save the designation to file</summary>
		/// <returns>The Xml writer object if successful</returns>
		public override bool Save()
		{
			XmlTextWriter	xmlWriter = null;
			XmlNode			xmlChild = null;
			
			//	Construct the full path specification
			GetFileSpec();
			if(m_strFileSpec.Length == 0) return false;

			//	Should we delete the existing file?
			if(System.IO.File.Exists(m_strFileSpec) == true)
			{
				try { System.IO.File.Delete(m_strFileSpec); }
				catch {}
			}
			
			try
			{
				//	Open the file
				if(Open(true) == true)
				{
					Debug.Assert(m_xmlDocument != null);
					Debug.Assert(m_xmlRoot != null);
					
					//	Get the node for the product update itself
					if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					{
						m_xmlRoot.AppendChild(xmlChild);
						
						if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
						{
							xmlWriter.Formatting = Formatting.Indented;
							
							m_xmlDocument.Save(xmlWriter);
							xmlWriter.Close();
							
							//	Close the document without destroying the local collections
							Close(false);
							
							return true;
						}
						
					}// if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					
				} // if(Open(true) == true)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
			}
			
			return false;
		
		}//	public override bool Save()
		
		/// <summary>This method creates an xml node using the object's properties</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="strName">The name assigned to the node</param>
		///	<returns>An Xml node that represents the object</returns>
		public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		{
			XmlElement	xmlElement  = null;
			XmlNode		xmlChild	= null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_PRODUCT_UPDATE_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_SOURCE, Source) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_CREATED_BY, CreatedBy) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_CREATED_ON, CreatedOn) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_MODIFIED_BY, ModifiedBy) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_MODIFIED_ON, ModifiedOn) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_RETRIEVED_BY, RetrievedBy) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_RETRIEVED_ON, RetrievedOn) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_PRODUCT_UPDATE_ATTRIBUTE_APP_FILENAME, AppFilename) == false)
						break;
						
					if((Updates != null) && (Updates.Count > 0))
					{
						foreach(CXmlUpdate O in Updates)
						{
							if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
							{
								xmlElement.AppendChild(xmlChild);
							}
						}
					}
					
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			//	Do the base class processing first
			base.SetErrorStrings();
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to open the product update: filename = %1");
			aStrings.Add("%1 is not a valid XML designation. It does not contain a valid product update node.");
			aStrings.Add("An exception was raised while attempting to set the product update properties");
			aStrings.Add("An exception was raised while attempting to fast fill the product update: filename = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the designation update text");
			
		}
		
		
		#endregion Protected Methods
		
		#region Properties
		
		//	Source URL from which the file was downloaded
		public string Source
		{
			get{ return m_strSource; }
			set{ m_strSource = value; }
		}
		
		//	Collection of available updates
		public CXmlUpdates Updates
		{
			get{ return m_xmlUpdates; }
		}
		
		//	Name of the person that created the file
		public string CreatedBy
		{
			get{ return m_strCreatedBy; }
			set{ m_strCreatedBy = value; }
		}
		
		//	Date the file was created
		public string CreatedOn
		{
			get{ return m_strCreatedOn; }
			set{ m_strCreatedOn = value; }
		}
		
		//	Name of the person that last modified the file
		public string ModifiedBy
		{
			get{ return m_strModifiedBy; }
			set{ m_strModifiedBy = value; }
		}
		
		//	Date the file was last modified
		public string ModifiedOn
		{
			get{ return m_strModifiedOn; }
			set{ m_strModifiedOn = value; }
		}
		
		//	Name of the person that downloaded the file
		public string RetrievedBy
		{
			get{ return m_strRetrievedBy; }
			set{ m_strRetrievedBy = value; }
		}
		
		//	Date the file was downloaded
		public string RetrievedOn
		{
			get{ return m_strRetrievedOn; }
			set{ m_strRetrievedOn = value; }
		}
		
		//	Filename of the host application
		public string AppFilename
		{
			get{ return m_strAppFilename; }
			set{ m_strAppFilename = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlProductUpdate

}// namespace FTI.Shared.Xml
