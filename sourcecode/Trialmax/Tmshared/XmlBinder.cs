using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML binder</summary>
	public class CXmlBinder : CXmlFile
	{
		#region Constants
		
		private const string XML_BINDER_ROOT_NAME					= "trialmax";
		private const string XML_BINDER_ELEMENT_NAME				= "binder";

		private const string XML_BINDER_ATTRIBUTE_NAME				= "name";
		private const string XML_BINDER_ATTRIBUTE_BARCODE			= "barcode";
		private const string XML_BINDER_ATTRIBUTE_DESCRIPTION		= "description";
		private const string XML_BINDER_ATTRIBUTE_DISPLAY_ORDER		= "displayOrder";
		private const string XML_BINDER_ATTRIBUTE_ATTRIBUTES		= "attributes";
		
		protected const int ERROR_LOAD_BINDER_EX					= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_BINDER_NODES					= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX					= (LAST_XML_FILE_ERROR + 3);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bounded to the Binders property</summary>
		private CTmaxBinderItems m_tmaxBinders = null;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlBinder() : base()
		{
			m_strRoot = XML_BINDER_ROOT_NAME;
			Extension = GetExtension();
		}
		
		/// <summary>This method is called to get the filter string used to initialize a file selection form</summary>
		/// <param name="bAllowAll">true to allow All Files option</param>
		/// <returns>The appropriate filter string</returns>
		new static public string GetFilter(bool bAllowAll)
		{
			if(bAllowAll == true)
				return "Binders (*.xmlb)|*.xmlb|All Files (*.*)|*.*";
			else
				return "Binders (*.xmlb)|*.xmlb";
		}
		
		/// <summary>This method is called to get the default file extension</summary>
		/// <returns>The default file extension</returns>
		new static public string GetExtension()
		{
			return "xmlb";
		}
		
		/// <summary>Called to clear the object's child collections</summary>
		public void Clear()
		{
			if(m_tmaxBinders != null)
			{
				m_tmaxBinders.Clear(true);
				m_tmaxBinders = null;
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
			
			// NOTE:	We do not clear the FileSpec value so that the user can set it
			//			once and then read / write at will
		}
			
		/// <summary>This method is called to save the script to file</summary>
		/// <returns>true if successful</returns>
		public override bool Save()
		{
			XmlTextWriter	xmlWriter = null;
			XmlNode			xmlChild = null;
			
			//	Construct the full path specification
			GetFileSpec();
			if(m_strFileSpec.Length == 0) return false;
			
			//	Delete the existing file
			if(System.IO.File.Exists(m_strFileSpec) == true)
			{
				try { System.IO.File.Delete(m_strFileSpec); }
				catch {}
			}
			
			try
			{
				//	Open the file
				if(Open(true) == false) return false;
			
				Debug.Assert(m_xmlDocument != null);
				Debug.Assert(m_xmlRoot != null);

				if(m_tmaxBinders != null)
				{
					foreach(CTmaxBinderItem O in m_tmaxBinders)
					{
						if((xmlChild = ToXmlNode(m_xmlDocument, O)) != null)
						{
							m_xmlRoot.AppendChild(xmlChild);
						}
					
					}// foreach(CTmaxBinderItem O in m_tmaxBinders)
						
				}
				else
				{
					return false;
				}
				
				//	Save the XML document to file	
				if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
				{
					xmlWriter.Formatting = System.Xml.Formatting.Indented;
					xmlWriter.Indentation = 4;
							
					m_xmlDocument.Save(xmlWriter);
					xmlWriter.Close();
							
					//	Close the document without destroying the local collections
					Close(false);
							
					return true;
							
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
			}
			
			return false;
		
		}//	public override bool Save()
		
		/// <summary>This method will set the object'properties using the specified node</summary>
		/// <param name="tmaxBinderItem">The object to be initialized</param>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(CTmaxBinderItem tmaxBinderItem, XmlNode xmlNode)
		{
			XPathNavigator	xpNavigator = null;
			bool			bSuccessful = false;
			CTmaxBinderItem	tmaxChild = null;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				if((xpNavigator = xmlNode.CreateNavigator()) != null)
				{
					if(SetProperties(tmaxBinderItem, xpNavigator) == true)
					{
						bSuccessful = true;
						
						//	There may be child nodes if this is not a media reference
						if(tmaxBinderItem.IsMedia == false)
						{
							if((tmaxBinderItem.Children != null) && (xmlNode.ChildNodes != null))
							{
								foreach(XmlNode O in xmlNode.ChildNodes)
								{
									tmaxChild = new CTmaxBinderItem();
									if(SetProperties(tmaxChild, O) == true)
										tmaxBinderItem.Children.Add(tmaxChild);
								}
							
							}// if((tmaxBinderItem.Children != null) && (xmlNode.ChildNodes != null))
							
						}// if(tmaxBinderItem.IsMedia == false)
					
					}// if(SetProperties(tmaxBinderItem, xpNavigator) == true)
					
				}// if((xpNavigator = xmlNode.CreateNavigator()) != null)

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool SetProperties(XmlNode xmlNode)
		
		/// <summary>This method will set the link properties using the specified navigator</summary>
		/// <param name="tmaxBinderItem">The object to be initialized</param>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(CTmaxBinderItem tmaxBinderItem, XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_BINDER_ATTRIBUTE_BARCODE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					tmaxBinderItem.Barcode = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_BINDER_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					tmaxBinderItem.Name = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_BINDER_ATTRIBUTE_DESCRIPTION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					tmaxBinderItem.Description = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_BINDER_ATTRIBUTE_DISPLAY_ORDER,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					tmaxBinderItem.DisplayOrder = System.Convert.ToInt32(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_BINDER_ATTRIBUTE_ATTRIBUTES,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					tmaxBinderItem.Attributes = System.Convert.ToInt32(strAttribute);

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method is called to open the specified file and load the contents</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		/// <param name="bCreate">True to allow creation of the file</param>
		/// <returns>true if successful</returns>
		public bool Load(string strFileSpec, bool bCreate)
		{
			XmlNodeList		xmlBinders = null;
			CTmaxBinderItem	tmaxBinderItem = null;
			bool			bSuccessful = false;
			
			//	Clear the existing values
			Clear();

			//	Open the file
			if(Open(strFileSpec, bCreate) == false) return false;
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlRoot != null);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Get the binder nodes
					if((xmlBinders = m_xmlRoot.SelectNodes(XML_BINDER_ELEMENT_NAME)) == null)
					{
						//	Is this a new file?
						if(bCreate == true)
						{
							//	We don't expect the node to 
							//	appear in a new file
							bSuccessful = true;
						}
						else
						{
							m_tmaxEventSource.FireError(this, "Load", m_tmaxErrorBuilder.Message(ERROR_NO_BINDER_NODES, m_strFileSpec));
						}
						break;
					}
					
					//	Process the top level binder nodes
					foreach(XmlNode O in xmlBinders)
					{
						//	Create a new binder object and set its properties
						tmaxBinderItem = new CTmaxBinderItem();
					
						//	Set the script properties
						if(SetProperties(tmaxBinderItem, O) == true)
							this.Binders.Add(tmaxBinderItem);
							
					}// foreach(XmlNode O in xmlBinders)
					
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Load", m_tmaxErrorBuilder.Message(ERROR_LOAD_BINDER_EX, this.FileSpec), Ex);
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
			
		}//	public bool Load(string strFileSpec, bool bCreate)
		
		/// <summary>The method is called to get the reference to the application binders collection</summary>
		/// <returns>The collection of binder descriptors</returns>
		public CTmaxBinderItems GetBinders()
		{
			if(m_tmaxBinders == null)
			{
				m_tmaxBinders = new CTmaxBinderItems();
				this.EventSource.Attach(m_tmaxBinders.EventSource);
			}
			
			return m_tmaxBinders;
			
		}// public CTmaxBinderItems GetBinders()
		
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
			aStrings.Add("An exception was raised while attempting to open the XML binder: filename = %1");
			aStrings.Add("%1 is not a valid XML binder. It does not contain any valid binder nodes.");
			aStrings.Add("An exception was raised while attempting to set the XML binder properties");
			
		}// protected override void SetErrorStrings()
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method creates an xml node using the specified binder item</summary>
		/// <param name="xmlDocument">Xml document object to which the node will be added</param>
		/// <param name="tmaxBinderItem">The binder item to be converted to XML</param>
		///	<returns>An Xml node that represents the object</returns>
		public XmlNode ToXmlNode(XmlDocument xmlDocument, CTmaxBinderItem tmaxBinderItem)
		{
			XmlElement	xmlElement  = null;
			XmlNode		xmlChild	= null;
			bool		bSuccessful = false;
			string		strElementName = "";
						
			strElementName = XML_BINDER_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(tmaxBinderItem.IsMedia == true)
					{
						if(AddAttribute(xmlElement, XML_BINDER_ATTRIBUTE_BARCODE, tmaxBinderItem.Barcode) == false)
							break;
					}
						
					if(tmaxBinderItem.IsMedia == false)
					{
						if(AddAttribute(xmlElement, XML_BINDER_ATTRIBUTE_NAME, tmaxBinderItem.Name) == false)
							break;
					}
						
					if(tmaxBinderItem.Description.Length > 0)
					{
						if(AddAttribute(xmlElement, XML_BINDER_ATTRIBUTE_DESCRIPTION, tmaxBinderItem.Description) == false)
							break;
					}
						
					if(AddAttribute(xmlElement, XML_BINDER_ATTRIBUTE_DISPLAY_ORDER, tmaxBinderItem.DisplayOrder) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_BINDER_ATTRIBUTE_ATTRIBUTES, tmaxBinderItem.Attributes) == false)
						break;
						
					//	Get the subbinders
					if((tmaxBinderItem.IsMedia == false) && (tmaxBinderItem.Children != null))
					{
						foreach(CTmaxBinderItem O in tmaxBinderItem.Children)
						{
							if((xmlChild = ToXmlNode(xmlDocument, O)) != null)
							{
								xmlElement.AppendChild(xmlChild);
							}
						}
										
					}// if((tmaxBinderItem.IsMedia == false) && (tmaxBinderItem.Children != null))
					
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public XmlNode ToXmlNode(XmlDocument xmlDocument, CTmaxBinderItem tmaxBinderItem)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The collection of binder descriptors contained in the file</summary>
		public CTmaxBinderItems Binders
		{
			get{ return GetBinders(); }
			set{ m_tmaxBinders = value; }
		}
		
		#endregion Properties
		
	}// public class CXmlBinder
		
}// namespace FTI.Shared.Xml
