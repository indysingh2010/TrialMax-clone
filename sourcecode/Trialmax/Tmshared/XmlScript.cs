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
	/// <summary>This class manages the information associated with an XML deposition link</summary>
	public class CXmlScript : CXmlFile, ITmaxScriptBoxCtrl
	{
		#region Constants
		
		public const string XML_SCRIPT_ROOT_NAME					= "trialmax";
		private const string XML_SCRIPT_ELEMENT_NAME				= "script";

		private const string XML_SCRIPT_ATTRIBUTE_XML_SCRIPT_FORMAT = "xmlFormat";
		private const string XML_SCRIPT_ATTRIBUTE_MEDIA_ID			= "mediaId";
		private const string XML_SCRIPT_ATTRIBUTE_NAME				= "name";
		private const string XML_SCRIPT_ATTRIBUTE_SOURCE_FILESPEC	= "sourceFileSpec";
		
		protected const int ERROR_OPEN_SCRIPT_EX					= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_SCRIPT_NODE					= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX					= (LAST_XML_FILE_ERROR + 3);
		protected const int	ERROR_FAST_FILL_EX						= (LAST_XML_FILE_ERROR + 4);
		protected const int	ERROR_SCRIPT_NOT_FOUND					= (LAST_XML_FILE_ERROR + 5);
		protected const int	ERROR_GET_DEPOSITIONS_EX				= (LAST_XML_FILE_ERROR + 6);
		protected const int	ERROR_GET_DESIGNATIONS_EX				= (LAST_XML_FILE_ERROR + 7);
		protected const int	ERROR_SET_SORT_ORDERS_EX				= (LAST_XML_FILE_ERROR + 8);
		protected const int	ERROR_ADD_EX							= (LAST_XML_FILE_ERROR + 9);
		protected const int	ERROR_INSERT_EX							= (LAST_XML_FILE_ERROR + 10);
		protected const int	ERROR_REMOVE_EX							= (LAST_XML_FILE_ERROR + 11);
		protected const int	ERROR_REORDER_EX						= (LAST_XML_FILE_ERROR + 12);
		protected const int	ERROR_EDIT_EX							= (LAST_XML_FILE_ERROR + 13);
		protected const int	ERROR_EDIT_EXTENTS_EX					= (LAST_XML_FILE_ERROR + 14);
		protected const int	ERROR_EXCLUDE_EX						= (LAST_XML_FILE_ERROR + 15);
		protected const int	ERROR_SPLIT_EX							= (LAST_XML_FILE_ERROR + 16);
		protected const int ERROR_INVALID_EDIT_RANGE				= (LAST_XML_FILE_ERROR + 17);
		protected const int	ERROR_CHECK_RANGE_EX					= (LAST_XML_FILE_ERROR + 18);
		protected const int	ERROR_GET_SCENES_EX						= (LAST_XML_FILE_ERROR + 19);
		protected const int	ERROR_ADD_SCENE_EX						= (LAST_XML_FILE_ERROR + 20);
		protected const int	ERROR_CREATE_XML_SCENES_EX				= (LAST_XML_FILE_ERROR + 21);
		protected const int	ERROR_APPLY_MERGE_METHOD_EX				= (LAST_XML_FILE_ERROR + 22);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bounded to the MediaId property</summary>
		private string m_strMediaId = "";	
		
		/// <summary>This member is bounded to the Name property</summary>
		private string m_strName = "";	
		
		/// <summary>This member is bounded to the SourceFileSpec property</summary>
		private string m_strSourceFileSpec = "";	
		
		/// <summary>This member is bounded to the Saved property</summary>
		private bool m_bSaved = false;	
		
		/// <summary>Private member bound to the XmlDepositions property</summary>
		private CXmlDepositions m_xmlDepositions = new CXmlDepositions();
		
		/// <summary>Private member bound to the XmlDesignations property</summary>
		private CXmlDesignations m_xmlDesignations = new CXmlDesignations();
		
		/// <summary>Private member bound to the XmlScenes property</summary>
		private CXmlScenes m_xmlScenes = new CXmlScenes();
		
		/// <summary>Private member bound to the XmlScriptFormat property</summary>
		private TmaxXmlScriptFormats m_eXmlScriptFormat = TmaxXmlScriptFormats.Unknown;
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlScript() : base()
		{
			m_strRoot = XML_SCRIPT_ROOT_NAME;
			Extension = GetExtension();

			m_tmaxEventSource.Attach(m_xmlDesignations.EventSource);
			m_xmlDesignations.Comparer = new CXmlBaseSorter();
			m_xmlDesignations.KeepSorted = false;

			m_tmaxEventSource.Attach(m_xmlScenes.EventSource);
			m_xmlScenes.Comparer = new CXmlBaseSorter();
			m_xmlScenes.KeepSorted = false;
		}
		
		/// <summary>Copy constructor</summary>
		/// <param name="xmlScript">Source object to be copied</param>
		public CXmlScript(CXmlScript xmlScript) : base()
		{
			m_strRoot = XML_SCRIPT_ROOT_NAME;
			Extension = GetExtension();

			Debug.Assert(xmlScript != null);
			
			if(xmlScript != null)
				Copy(xmlScript);
		}
		
		/// <summary>This method is called to get the filter string used to initialize a file selection form</summary>
		/// <param name="bAllowAll">true to allow All Files option</param>
		/// <returns>The appropriate filter string</returns>
		new static public string GetFilter(bool bAllowAll)
		{
			if(bAllowAll == true)
				return "Scripts (*.xmls,*.xmlv)|*.xmls;*.xmlv|All Files (*.*)|*.*";
			else
				return "Scripts (*.xmls,*.xmlv)|*.xmls;*.xmlv";
		}
		
		/// <returns>The appropriate filter string</returns>
		/// <param name="eFormat">The format of the desired file</param>
		/// <param name="bAllowAll">true to allow All Files option</param>
		/// <returns>The default filter string</returns>
		static public string GetFilter(TmaxXmlScriptFormats eFormat, bool bAllowAll)
		{
			switch(eFormat)
			{
				case TmaxXmlScriptFormats.VideoViewer:
				
					if(bAllowAll == true)
						return "Scripts (*.xmlv)|*.xmlv|All Files (*.*)|*.*";
					else
						return "Scripts (*.xmlv)|*.xmlv";
					
				case TmaxXmlScriptFormats.Manager:
				default:
				
					if(bAllowAll == true)
						return "Scripts (*.xmls)|*.xmls|All Files (*.*)|*.*";
					else
						return "Scripts (*.xmls)|*.xmls";
			}
			
		}// new static public string GetFilter(TmaxXmlScriptFormats eFormat, bool bAllowAll)
		
		/// <summary>This method is called to get the default file extension</summary>
		/// <param name="eFormat">The format of the desired file</param>
		/// <returns>The default file extension</returns>
		static public string GetExtension(TmaxXmlScriptFormats eFormat)
		{
			switch(eFormat)
			{
				case TmaxXmlScriptFormats.VideoViewer:
				
					return "xmlv";
					
				case TmaxXmlScriptFormats.Manager:
				default:
				
					return "xmls";
			}

		}// static public string GetExtension(TmaxXmlScriptFormats eFormat)
		
		/// <summary>This method retrieves the default display text for the node</summary>
		/// <returns>The default display text string</returns>
		override public string GetDisplayString()
		{
			return this.Name;
			
		}// override public string GetDisplayString()
		
		/// <summary>This function is called to compare the specified object to this object</summary>
		/// <param name="xmlBase">Object to be compared</param>
		/// <param name="iMode">Mode identifier defined by derived class</param>
		/// <returns>-1 if this less than xmlBase, 0 if equal, 1 if xmlBase greater than this</returns>
		override public int Compare(CXmlBase xmlBase, int iMode)
		{
			try
			{
				return CTmaxToolbox.Compare(this.MediaId, ((CXmlScript)xmlBase).MediaId, true);
			}
			catch
			{
				return -1;
			}
			
		}// virtual public int Compare(CXmlBase xmlBase)
		
		/// <summary>Called to clear the object's child collections</summary>
		public void Clear()
		{
			//	Reset the property values
			m_strMediaId = "";
			m_strName = "";
			m_strSourceFileSpec = "";
			
			//	Flush the child collections
			m_xmlDepositions.Clear();
			m_xmlDesignations.Clear();
			m_xmlScenes.Clear();
			
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

				//	Get the XML node for this script
				if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
				{
					m_xmlRoot.AppendChild(xmlChild);
				}
				else
				{
					return false;
				}
				
				//	Should we add the source depositions?
				if((m_xmlDepositions != null) && (m_xmlDepositions.Count > 0))
				{
					foreach(CXmlDeposition O in m_xmlDepositions)
					{
						//	Get the XML node for the deposition
						if((xmlChild = O.ToXmlNode(m_xmlDocument)) != null)
						{
							m_xmlRoot.AppendChild(xmlChild);
						}
						else
						{
							return false;
						}
					
					}// foreach(CXmlDeposition O in m_xmlDepositions)
					
				}// if((m_xmlDepositions != null) && (m_xmlDepositions.Count > 0))
				
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
		
		/// <summary>Called to copy the properties of the source node</summary>
		/// <param name="xmlScript">The source object to be copied</param>
		/// <param name="bDesignations">true to copy the source designations</param>
		/// <param name="bScenes">true to copy the source scenes</param>
		/// <param name="bDepositions">true to copy the source depositions</param>
		public void Copy(CXmlScript xmlScript, bool bDesignations, bool bScenes, bool bDepositions)
		{
			//	Copy the base class members
			base.Copy(xmlScript as CXmlBase);
			
			XmlScriptFormat = xmlScript.XmlScriptFormat;
			MediaId = xmlScript.MediaId;
			Name = xmlScript.Name;
			SourceFileSpec = xmlScript.SourceFileSpec;
			XmlDeposition = xmlScript.XmlDeposition;
			Saved = xmlScript.Saved;
			
			//	Clear the child collections
			m_xmlDesignations.Clear();
			m_xmlScenes.Clear();
			m_xmlDepositions.Clear();
			
			if(bDesignations == true)
			{
				foreach(CXmlDesignation O in xmlScript.XmlDesignations)
					m_xmlDesignations.Add(new CXmlDesignation(O));
			}
			
			if(bScenes == true)
			{
				foreach(CXmlScene O in xmlScript.XmlScenes)
					m_xmlScenes.Add(new CXmlScene(O));
			}
			
			if(bDepositions == true)
			{
				foreach(CXmlDeposition O in xmlScript.XmlDepositions)
					m_xmlDepositions.Add(new CXmlDeposition(O));
			}
			
		}// public void Copy(CXmlScript xmlScript, bool bDesignations, bool bScenes, bool bDepositions)
		
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
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
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
				strAttribute = xpNavigator.GetAttribute(XML_SCRIPT_ATTRIBUTE_XML_SCRIPT_FORMAT,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_eXmlScriptFormat = CTmaxToolbox.GetFormatFromString(strAttribute);

				strAttribute = xpNavigator.GetAttribute(XML_SCRIPT_ATTRIBUTE_MEDIA_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strMediaId = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SCRIPT_ATTRIBUTE_NAME,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strName = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_SCRIPT_ATTRIBUTE_SOURCE_FILESPEC,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSourceFileSpec = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
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
			XmlNode		xmlChild	= null;
			bool		bSuccessful = false;
			string		strElementName = "";
			
			if((strName != null) && (strName.Length > 0))
				strElementName = strName;
			else
				strElementName = XML_SCRIPT_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_SCRIPT_ATTRIBUTE_XML_SCRIPT_FORMAT, m_eXmlScriptFormat.ToString()) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCRIPT_ATTRIBUTE_MEDIA_ID, MediaId) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCRIPT_ATTRIBUTE_NAME, Name) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_SCRIPT_ATTRIBUTE_SOURCE_FILESPEC, SourceFileSpec) == false)
						break;
						
					//	Get the scenes if this is a Manager formatted script
					if(this.XmlScriptFormat == TmaxXmlScriptFormats.Manager)
					{
						if((XmlScenes != null) && (XmlScenes.Count > 0))
						{
							foreach(CXmlScene O in XmlScenes)
							{
								if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
								{
									xmlElement.AppendChild(xmlChild);
								}
							}
					
						}// if((XmlScenes != null) && (XmlScenes.Count > 0))
										
					}// if(this.XmlScriptFormat == TmaxXmlScriptFormats.Manager)
					
					if((XmlDesignations != null) && (XmlDesignations.Count > 0))
					{
						foreach(CXmlDesignation O in XmlDesignations)
						{
							if((xmlChild = O.ToXmlNode(xmlDocument)) != null)
							{
								xmlElement.AppendChild(xmlChild);
							}
						}
					
					}// if((XmlDesignations != null) && (XmlDesignations.Count > 0))
										
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>This method is called to initialize the object using the specified XPath document</summary>
		/// <param name="xpDocument">The XPath document containing the desposition</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <param name="bDepositions">true to load the source depositions</param>
		/// <returns>true if successful</returns>
		public bool FastFill(XPathDocument xpDocument, string strParentPath, bool bDeposition)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			string				strXPath = "";
			bool				bScriptFound = false;
			bool				bDepositionFound = false;
			
			if(xpDocument == null) return false;
			
			try
			{
				//	Clear the existing values
				Clear();
				
				//	Assign default parent path if necessary
				//
				//	NOTE:	GetDeposition() also uses the parent path
				if((strParentPath == null) || (strParentPath.Length == 0))
					strParentPath = String.Format("{0}", XML_SCRIPT_ROOT_NAME);
				
				//	Build the query path for the script node
				strXPath = String.Format("{0}/{1}", strParentPath, XML_SCRIPT_ELEMENT_NAME);

				//	Get the object's properties
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				if(xpIterator.Count > 0)
				{
					//	We found the script node
					xpIterator.MoveNext();
					bScriptFound = true;
					
					//	Set the script properties
					SetProperties(xpIterator.Current);
							
					//	Get the designations owned by this script
					GetDesignations(xpDocument, strXPath);
					
					//	Get the scenes if this is a Manager formatted script
					if(this.XmlScriptFormat == TmaxXmlScriptFormats.Manager)
						GetScenes(xpDocument, strXPath);
				}
				
				//	Get the source depositions if requested
				//
				//	NOTE:	The deposition parent is the same as the script parent
				//			In fact, this may actually be a deposition that's being
				//			converted to a script
				if(bDeposition == true)
					bDepositionFound = GetDepositions(xpDocument, strParentPath);
				
				//	Must have the Script or Deposition node
				if((bScriptFound == false) && (bDepositionFound == false))
				{
					m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_NO_SCRIPT_NODE, this.GetFileSpec()));
					return false;
				}

				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, this.GetFileSpec()), Ex);
			}
			
			//	Must have been a problem
			return false;
			
		}// public bool FastFill(XPathDocument xpDocument, string strParentPath, bool bDeposition)
		
		/// <summary>This method is called to initialize the object using the specified file</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file containing the script</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <param name="bDeposition">true to initialize the source deposition</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, string strParentPath, bool bDeposition)
		{
			XPathDocument xpDocument = null;

			//	Make sure the file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_SCRIPT_NOT_FOUND, strFileSpec));
				return false;
			}
			
			try
			{
				//	Load the file
				xpDocument = new System.Xml.XPath.XPathDocument(strFileSpec);
				
				//	Fill the collections
				SetFileProps(strFileSpec);
				return FastFill(xpDocument, strParentPath, bDeposition);
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "FastFill", m_tmaxErrorBuilder.Message(ERROR_FAST_FILL_EX, strFileSpec), Ex);
			}
			
			//	Must have been a problem
			return false;
			
		}// public bool FastFill(string strFileSpec, string strParentPath, bool bDeposition)
		
		/// <summary>This method is called to initialize the object using the specified XPath document</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file containing the script</param>
		/// <param name="bDeposition">true to initialize the source deposition</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec, bool bDeposition)
		{
			return FastFill(strFileSpec, "", bDeposition);
		}
		
		/// <summary>The method uses XPath queries against the specified file to initialize the objec</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <returns>true if successful</returns>
		public bool FastFill(string strFileSpec)
		{
			return FastFill(strFileSpec, "", true);
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
					//	Get the deposition descriptor node
					if((xmlNode = m_xmlRoot.SelectSingleNode(XML_SCRIPT_ELEMENT_NAME)) == null)
					{
						//	Is this a new file?
						if(bCreate == true)
						{
							//	We don't expect the deposition node to 
							//	appear in a new file
							bSuccessful = true;
						}
						else
						{
							m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_SCRIPT_NODE, this.GetFileSpec()));
						}
						break;
					}
										
					//	Get the deposition properties
					if(SetProperties(xmlNode) == false)
						break;
						
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_SCRIPT_EX, this.GetFileSpec()), Ex);
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
			
		}//	public override bool Open(bool bCreate)
		
		/// <summary>Called to add the specified designation to the script</summary>
		/// <param name="xmlDesignation">The designation to be added to the script</param>
		/// <returns>True if successful</returns>
		public bool Add(CXmlDesignation xmlDesignation)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			
			try
			{
				//	Set the sort order value so that the new designation
				//	appears at the end of the collection
				xmlDesignation.XmlSortOrder = GetNextSortOrder();
				
				//	Add to the collection
				m_xmlDesignations.Add(xmlDesignation);
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Add(CXmlDesignation xmlDesignation)
		
		/// <summary>Called to add all designations in the specified collection to the script's collection</summary>
		public bool Add(CXmlDesignations xmlDesignations)
		{
			bool	bSuccessful = false;
			int		iSortOrder = 0;

			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;

			try
			{
				//	Get the first new sort order value
				iSortOrder = GetNextSortOrder();
				
				//	Assign the sort order and add each new designation
				foreach(CXmlDesignation O in xmlDesignations)
				{
					O.XmlSortOrder = iSortOrder;
					iSortOrder++;
					
					m_xmlDesignations.Add(O);
				}
							
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Add(CXmlDesignations xmlDesignations)
		
		/// <summary>Called to add the specified scene to the script</summary>
		/// <param name="xmlScene">The scene to be added to the script</param>
		/// <returns>True if successful</returns>
		public bool Add(CXmlScene xmlScene)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_xmlScenes != null);
			if(m_xmlScenes == null) return false;
			
			try
			{
				//	Add to the collection
				m_xmlScenes.Add(xmlScene);
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Add", m_tmaxErrorBuilder.Message(ERROR_ADD_SCENE_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Add(CXmlScene xmlScene)
		
		/// <summary>Called to add the specified designation scene to the script</summary>
		/// <param name="xmlScene">The scene that owns the designation</param>
		/// <param name="xmlDesignation">The designation bound to the specified scene</param>
		/// <returns>True if successful</returns>
		public bool Add(CXmlScene xmlScene, CXmlDesignation xmlDesignation)
		{
			//	Add the scene to the database
			if(Add(xmlScene) == false)
				return false;
				
			//	Add the designation
			if(xmlDesignation != null)
			{
				if(Add(xmlDesignation) == false)
					return false;
				
				//	Link the designation to the scene
				xmlScene.SourceId = (m_xmlDesignations.IndexOf(xmlDesignation)).ToString();
			
			}// if(xmlDesignation != null)
			
			return true;
			
		}// public bool Add(CXmlScene xmlScene, CXmlDesignation xmlDesignation)
		
		/// <summary>Called to insert all designations in the specified collection to the script's collection at the specified location</summary>
		/// <param name="xmlDesignations">The designations to be inserted into the script</param>
		/// <param name="bBefore">true to insert before the specified location</param>
		public bool Insert(CXmlDesignations xmlDesignations, CXmlDesignation xmlLocation, bool bBefore)
		{
			bool	bSuccessful = false;
			int		iIndex = -1;
			
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			
			try
			{
				//	Get the index of the insertion point
				iIndex = m_xmlDesignations.IndexOf(xmlLocation);
					
				//	Adjust the index if inserting after the specified location
				if((iIndex >= 0) && (bBefore == false)) iIndex++;
				
				//	Are we really adding the designations
				if((iIndex < 0) || (iIndex >= m_xmlDesignations.Count))
					return Add(xmlDesignations);
					
				//	Insert each new designation
				m_xmlDesignations.InsertRange(iIndex, xmlDesignations);
							
				//	Reset the sort order values
				SetSortOrders();
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Insert", m_tmaxErrorBuilder.Message(ERROR_INSERT_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Insert(CXmlDesignations xmlDesignations, CXmlDesignation xmlLocation, bool bBefore)
		
		/// <summary>Called to insert the specified designation to the script's collection at the specified location</summary>
		/// <param name="xmlDesignation">The designation to be inserted into the script</param>
		/// <param name="bBefore">true to insert before the specified location</param>
		public bool Insert(CXmlDesignation xmlDesignation, CXmlDesignation xmlLocation, bool bBefore)
		{
			CXmlDesignations xmlDesignations = new CXmlDesignations();
			
			xmlDesignations.Add(xmlDesignation);
			return Insert(xmlDesignations, xmlLocation, bBefore);
			
		}// public bool Insert(CXmlDesignation xmlDesignation, CXmlDesignation xmlLocation, bool bBefore)
		
		/// <summary>Called to remove the specified designation from the script</summary>
		public bool Remove(CXmlDesignation xmlDesignation)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			
			try
			{
				m_xmlDesignations.Remove(xmlDesignation);				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Remove", m_tmaxErrorBuilder.Message(ERROR_REMOVE_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Remove(CXmlDesignation xmlDesignation)
		
		/// <summary>Called to remove the specified designations from the script</summary>
		public bool Remove(CXmlDesignations xmlDesignations)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			
			try
			{
				foreach(CXmlDesignation O in xmlDesignations)
					Remove(O);
					
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Remove", m_tmaxErrorBuilder.Message(ERROR_REMOVE_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Remove(CXmlDesignations xmlDesignations)
		
		/// <summary>Called to reorder the script designations to match the order of the caller's collection</summary>
		public bool Reorder(CXmlDesignations xmlDesignations)
		{
			bool	bSuccessful = false;

			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;

			try
			{
				//	The collection sizes should match
				if(xmlDesignations.Count != m_xmlDesignations.Count)
				{
					m_tmaxEventSource.FireDiagnostic(this, "Reorder", "Collection sizes do not match");
					return false;
				}
				
				//	Clear the current collection
				m_xmlDesignations.Clear();
				
				//	Now populate with the new order
				foreach(CXmlDesignation O in xmlDesignations)
					m_xmlDesignations.Add(O);
					
				//	Update the sort order values
				SetSortOrders();
							
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Reorder", m_tmaxErrorBuilder.Message(ERROR_REORDER_EX), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Reorder(CXmlDesignations xmlDesignations)
		
		/// <summary>Called to edit the specified designation</summary>
		/// <param name="eMethod">Method to be used for editing the designation</param>
		/// <param name="xmlDesignation">The designation to be edited</param>
		/// <param name="lStartPL">The new start position</param>
		/// <param name="lStopPL">The new stop position</param>
		/// <param name="xmlAdded">Collection to store new designations resulting from the operation</param>
		/// <returns>true if successful</returns>
		public bool Edit(TmaxDesignationEditMethods eMethod, CXmlDesignation xmlDesignation, long lStartPL, long lStopPL, CXmlDesignations xmlAdded)
		{
			bool bSuccessful = false;
			
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			Debug.Assert(m_xmlDesignations.Contains(xmlDesignation) == true);
			if(m_xmlDesignations.Contains(xmlDesignation) == false) return false;
			
			try
			{
				//	Make sure the range is valid]
				if(CheckRange(lStartPL, lStopPL) == false) return false;
				
				//	What method?
				switch(eMethod)
				{
					case TmaxDesignationEditMethods.Extents:
								
						EditExtents(xmlDesignation, lStartPL, lStopPL, xmlAdded);
						break;
									
					case TmaxDesignationEditMethods.Exclude:
								
						Exclude(xmlDesignation, lStartPL, lStopPL, xmlAdded);
						break;
									
					case TmaxDesignationEditMethods.SplitBefore:
								
						Split(xmlDesignation, true, lStartPL, lStopPL, xmlAdded);
						break;
									
					case TmaxDesignationEditMethods.SplitAfter:
								
						Split(xmlDesignation, false, lStartPL, lStopPL, xmlAdded);
						break;
									
					default:
								
						break;
									
				}// switch(m_eMethod)

				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Edit", m_tmaxErrorBuilder.Message(ERROR_EDIT_EX, eMethod, xmlDesignation.Name), Ex);
			}
			
			return bSuccessful;
			
		}// public bool Edit(TmaxDesignationEditMethods eMethod, CXmlDesignation xmlDesignation, long lStartPL, long StopPL, CXmlDesignations xmlAdded)
		
		/// <summary>This method is called to get the deposition when the file is assumed to have only one deposition</summary>
		/// <returns>The embedded deposition</returns>
		/// <remarks>This method is used to get the active deposition when it is assumed that there is only one (Video Viewer)</remarks>
		public CXmlDeposition GetXmlDeposition()
		{
			if((m_xmlDepositions != null) && (m_xmlDepositions.Count > 0))
				return m_xmlDepositions[0];
			else
				return null;
					
		}// public CXmlDeposition GetXmlDeposition()
		
		/// <summary>This method is called to set the deposition when the file is assumed to have only one deposition</summary>
		/// <remarks>This method is used to set the active deposition when it is assumed that there is only one (Video Viewer)</remarks>
		public void SetXmlDeposition(CXmlDeposition xmlDeposition)
		{
			if(m_xmlDepositions != null)
			{
				m_xmlDepositions.Clear();
				if(xmlDeposition != null)
					m_xmlDepositions.Add(xmlDeposition);
			}
					
		}// public void SetXmlDeposition(CXmlDeposition xmlDeposition)
		
		/// <summary>This method is called to get the scene's source designation</summary>
		/// <param name="xmlScene">The scene that references the desired source</param>
		/// <returns>The source designation</returns>
		public CXmlDesignation GetSourceDesignation(CXmlScene xmlScene)
		{
			int	iIndex = 0;
			
			Debug.Assert(xmlScene != null);
			if(xmlScene == null) return null;
			
			//	The source id of the scene is actually an index
			if(this.XmlDesignations == null) return null;
			if(this.XmlDesignations.Count == 0) return null;
			if(xmlScene.SourceId.Length == 0) return null;
			try { iIndex = System.Convert.ToInt32(xmlScene.SourceId); }
			catch { return null; }
			
			//	Is the index within range
			if((iIndex >= 0) && (iIndex < this.XmlDesignations.Count))
				return this.XmlDesignations[iIndex];
			else
				return null;
					
		}// public CXmlDesignation GetSourceDesignation(CXmlScene xmlScene)
		
		/// <summary>This method is called to get the designation's source deposition</summary>
		/// <param name="xmlDesignation">The designation created from the source deposition</param>
		/// <returns>The source deposition</returns>
		public CXmlDeposition GetSourceDeposition(CXmlDesignation xmlDesignation)
		{
			CXmlDeposition xmlDeposition = null;
			
			Debug.Assert(xmlDesignation != null);
			if(xmlDesignation == null) return null;
			
			//	Do we have any depositions?
			if(this.XmlDepositions == null) return null;
			if(this.XmlDepositions.Count == 0) return null;
			
			//	Do we have a primary media id for the designation?
			if(xmlDesignation.PrimaryId.Length > 0)
			{
				xmlDeposition = this.XmlDepositions.Find(xmlDesignation.PrimaryId);	
			}
			else
			{
				//	It there's only one deposition assume its the source
				//
				//	NOTE:	This would be the case for a Video Viewer script
				if(this.XmlDepositions.Count == 1)
					xmlDeposition = this.XmlDepositions[0];
			}
			
			return xmlDeposition;
					
		}// public CXmlDeposition GetSourceDeposition(CXmlScene xmlScene)
		
		/// <summary>This method is called to get the scene's source deposition</summary>
		/// <param name="xmlScene">The scene that references the desired source</param>
		/// <returns>The source deposition</returns>
		public CXmlDeposition GetSourceDeposition(CXmlScene xmlScene)
		{
			CXmlDesignation xmlDesignation = null;
			CXmlDeposition	xmlDeposition = null;
		
			//	Get the designation bound to this scene
			if((xmlDesignation = GetSourceDesignation(xmlScene)) != null)
				xmlDeposition = GetSourceDeposition(xmlDesignation);		
		
			return xmlDeposition;
		
		}// public CXmlDeposition GetSourceDeposition(CXmlScene xmlScene)
		
		/// <summary>This method will create scenes for each of the designations in the script</summary>
		/// <returns>true if successful</returns>
		/// <remarks>This method is used by TmaxManager to make the file look like a Manager formatted file for import operations</remarks>
		public bool CreateXmlScenes()
		{
			CXmlScene	xmlScene = null;
			bool		bSuccessful = true;
			
			//	We assume there are no scenes when this method is called
			if(m_xmlScenes.Count > 0) return false;
			
			try
			{
				//	Create a scene for each designation
				for(int i = 0; i < m_xmlDesignations.Count; i++)
				{
					//	Create and initialize the scene
					xmlScene = new CXmlScene();
					xmlScene.SourceType = TmaxMediaTypes.Designation;
					xmlScene.SourceId = i.ToString();
					xmlScene.BarcodeId = (i + 1);
					
					//	Add to the collection
					if((bSuccessful = Add(xmlScene)) == false)
						break;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CreateXmlScenes", m_tmaxErrorBuilder.Message(ERROR_CREATE_XML_SCENES_EX), Ex);
				bSuccessful = false;
			}
			
			return bSuccessful;
			
		}// public bool CreateXmlScenes()
		
		/// <summary>This method is called to get the segment with the specified key</summary>
		/// <returns>The segment with the key value specified by the caller</returns>
		public CXmlSegment GetSegment(string strKey)
		{
			if(this.XmlDeposition != null)
				return this.XmlDeposition.GetSegment(strKey);
			else
				return null;
					
		}// public CXmlSegment GetSegment(string strKey)
		
		/// <summary>This method is called to merge the designation in the script using the specified method</summary>
		/// <param name="tmaxOptions">The collection of user defined options that control the operation</param>
		/// <returns>true if successful</returns>
		public bool ApplyMergeMethod(CTmaxImportOptions tmaxOptions)
		{
			bool		bSuccessful = true;
			CXmlScene	xmlPending = null;
			CXmlScenes	xmlMerged = null;
			
			//	Don't bother if no merging is required
			if(tmaxOptions.MergeDesignations == TmaxDesignationMergeMethods.None) return true;
			
			//	Don't bother if we don't have any designations
			if(this.XmlDesignations == null) return true;
			if(this.XmlDesignations.Count <= 1) return true;
		
			//	For now we assume there are also scenes available
			//	We will have to modify this if we add this capability to Video Viewer
			if(this.XmlScenes == null) return true;
			if(this.XmlScenes.Count <= 1) return true;
			
			try
			{
				//	Temporary collection to store merged designations
				xmlMerged = new CXmlScenes();
				
				for(int i = 0; i < this.XmlScenes.Count; i++)
				{
					//	Is this scene a designation?
					if(this.XmlScenes[i].SourceType == TmaxMediaTypes.Designation)
					{
						//	Do we have a pending designation?
						if(xmlPending != null)
						{
							//	Can we merge this one with the pending designation?
							if(Merge(xmlPending, this.XmlScenes[i], tmaxOptions) == true)
							{
								xmlMerged.Add(this.XmlScenes[i]);
							}
							else
							{
								xmlPending = this.XmlScenes[i];
							}
							
						}
						else
						{
							xmlPending = this.XmlScenes[i];
						}
						
					}
					else
					{
						xmlPending = null;
					
					}// if(this.XmlScenes[i].SourceType == TmaxMediaTypes.Designation)
				
				}// for(int i = 0; i < this.XmlScenes.Count; i++)
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "ApplyMergeMethod", m_tmaxErrorBuilder.Message(ERROR_APPLY_MERGE_METHOD_EX, tmaxOptions.MergeDesignations), Ex);
				bSuccessful = false;
			}
			
			//	Did we merge any scenes
			if((xmlMerged != null) && (xmlMerged.Count > 0))
			{
				//	Remove each of the scenes
				//
				//	NOTE:	We do NOT remove the source designations because that
				//			would mean we have to change the SourceId for each remaining
				//			scene since the SourceId is actually an index into the 
				//			XmlDesignations collection
				foreach(CXmlScene O in xmlMerged)
					this.XmlScenes.Remove(O);
					
			}// if((xmlMerged != null) && (xmlMerged.Count > 0))
			
			return bSuccessful;
			
		}// public bool ApplyMergeMethod(TmaxDesignationMergeMethods eMethod)
		
		/// <summary>This function is called to get the Name bound to the object</summary>
		/// <returns>The object's name</returns>
		string ITmaxScriptBoxCtrl.GetName()
		{
			return this.Name;
			
		}// string ITmaxScriptBoxCtrl.GetName()
		
		/// <summary>This function is called to get the MediaId bound to the object</summary>
		/// <returns>The object's name</returns>
		string ITmaxScriptBoxCtrl.GetMediaId()
		{
			return this.MediaId;
			
		}// string ITmaxScriptBoxCtrl.GetMediaId()
		
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
			aStrings.Add("An exception was raised while attempting to open the XML script: filename = %1");
			aStrings.Add("%1 is not a valid XML script. It does not contain a valid script node.");
			aStrings.Add("An exception was raised while attempting to set the XML script properties");
			aStrings.Add("An exception was raised while attempting to fast fill the script: filename = %1");
			aStrings.Add("Unable to locate the XML file containing the script information: filename = %1");
			
			aStrings.Add("An exception was raised while attempting to retrieve the XML script's source depositions: filename = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the XML script's designations: filename = %1");
			aStrings.Add("An exception was raised while attempting to set the sort order for the script's designations");
			aStrings.Add("An exception was raised while attempting to add a designation to the script.");
			aStrings.Add("An exception was raised while attempting to insert a designation into the script.");
			
			aStrings.Add("An exception was raised while attempting to remove a designation in the script.");
			aStrings.Add("An exception was raised while attempting to reorder the designations.");
			aStrings.Add("An exception was raised while attempting to edit (%1) the specified designation: %2");
			aStrings.Add("An exception was raised while attempting to edit the extents of the specified designation: name = %1  start = %2  stop = %3");
			aStrings.Add("An exception was raised while attempting to exclude text in the specified designation: name = %1  start = %2  stop = %3");
		
			aStrings.Add("An exception was raised while attempting to split the specified designation: name = %1  start = %2  stop = %3");
			aStrings.Add("The selection range specified for the operation is not valid: start = %1  stop = %2");
			aStrings.Add("An exception was raised while attempting to validate the selection range for the operation: start = %1  stop = %2");
			aStrings.Add("An exception was raised while attempting to retrieve the XML script's scenes: filename = %1");
			aStrings.Add("An exception was raised while attempting to add a scene to the XML script.");

			aStrings.Add("An exception was raised while attempting to create the XML scenes.");
			aStrings.Add("An exception was raised while attempting to merge the designations: method = %1");

		}// protected override void SetErrorStrings()
		
		#endregion Protected Methods
		
		#region Private Methods
		
		/// <summary>This method is called to merge the specified designation scenes</summary>
		/// <param name="xmlScene1">The first scene to be merged</param>
		/// <param name="xmlScene2">The first scene to be merged</param>
		/// <param name="tmaxOptions">The collection of user defined import options</param>
		/// <returns>true if they have been merged</returns>
		public bool Merge(CXmlScene xmlScene1, CXmlScene xmlScene2, CTmaxImportOptions tmaxOptions)
		{
			CXmlDesignation xmlDesignation1 = null;
			CXmlDesignation xmlDesignation2 = null;
			
			if((xmlDesignation1 = GetSourceDesignation(xmlScene1)) == null)
				return false;
			if((xmlDesignation2 = GetSourceDesignation(xmlScene2)) == null)
				return false;
			else
				return Merge(xmlDesignation1, xmlDesignation2, tmaxOptions);
			
		}// public bool Merge(CXmlScene xmlScene1, CXmlScene xmlScene2, TmaxSceneMergeMethods eMethod)
		
		/// <summary>This method is called to merge the specified designations</summary>
		/// <param name="xmlDesignation1">The first designation to be merged</param>
		/// <param name="xmlDesignation2">The first designation to be merged</param>
		/// <param name="tmaxOptions">The collection of user defined options</param>
		/// <returns>true if they have been merged</returns>
		public bool Merge(CXmlDesignation xmlDesignation1, CXmlDesignation xmlDesignation2, CTmaxImportOptions tmaxOptions)
		{
			bool			bMerged = false;
			CXmlDeposition	xmlDeposition = null;
			int				iLinesPerPage = 24;	//	Initialize to default
			long			lPage1 = 0;
			long			lPage2 = 0;
			int				iLine1 = 0;
			int				iLine2 = 0;
			
			//	Check the basics first
			if(xmlDesignation1.PrimaryId != xmlDesignation2.PrimaryId) return false;
			if(xmlDesignation1.Segment != xmlDesignation2.Segment) return false;
			if(xmlDesignation1.Highlighter != xmlDesignation2.Highlighter) return false;
			
			//	Can't merge if endpoints have been tuned
			if(xmlDesignation1.StopTuned == true) return false;
			if(xmlDesignation2.StartTuned == true) return false;
			
			//	Get the maximum number of lines per page
			if((xmlDeposition = GetSourceDeposition(xmlDesignation1)) != null)
				iLinesPerPage = xmlDeposition.LinesPerPage;
				
			//	Get the page numbers
			lPage1 = CTmaxToolbox.PLToPage(xmlDesignation1.LastPL);
			lPage2 = CTmaxToolbox.PLToPage(xmlDesignation2.FirstPL);
			
			//	Get the line numbers
			iLine1 = CTmaxToolbox.PLToLine(xmlDesignation1.LastPL);
			iLine2 = CTmaxToolbox.PLToLine(xmlDesignation2.FirstPL);
			
			if(tmaxOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentPages)
			{
				//	Does first designation end on the last line of the page
				if(iLine1 >= iLinesPerPage)
				{
					//	Are these adjacent pages?
					if(lPage2 == (lPage1 + 1))
					{
						if(iLine2 == 1)
							bMerged = true;
						else if((iLine2 == 2) && (tmaxOptions.IgnoreFirstLine == true))
							bMerged = true;

					}// if(lPage2 == (lPage1 + 1))

				}// if(iLine1 >= iLinesPerPage)
			
			}
			else if(tmaxOptions.MergeDesignations == TmaxDesignationMergeMethods.AdjacentLines)
			{
				//	Are they on the same page?
				if(lPage1 == lPage2)
				{
					if(iLine2 == (iLine1 + 1))
						bMerged = true;
				}
				//	Adjacent pages?
				else if(lPage2 == (lPage1 + 1))
				{
					if((iLine1 >= iLinesPerPage) && (iLine2 == 1))
						bMerged = true;
				}
				
			}// else if(eMethod == TmaxDesignationMergeMethods.AdjacentLines)

			//	Do we need to merge the designations?
			if(bMerged == true)
			{
				xmlDesignation1.Stop = xmlDesignation2.Stop;
				xmlDesignation1.LastPL = xmlDesignation2.LastPL;
				
				foreach(CXmlTranscript O in xmlDesignation2.Transcripts)
					xmlDesignation1.Transcripts.Add(O);
			
				if(xmlDeposition != null)
					xmlDesignation1.SetNameFromExtents(xmlDeposition);
				else
					xmlDesignation1.SetNameFromExtents("");

			}// if(bMerged == true)
			
			return bMerged;
			
		}// public bool Merge(CXmlDesignation xmlDesignation1, CXmlDesignation xmlDesignation2, TmaxDesignationMergeMethods eMethod)
		
		/// <summary>Called to get the XML designations for this script</summary>
		/// <param name="xpDocument">The XPath document containing the source deposition</param>
		/// <param name="strScriptPath">The path to the parent script element</param>
		/// <returns>true if successful</returns>
		private bool GetDesignations(XPathDocument xpDocument, string strScriptPath)
		{
			bool				bSuccessful = false;
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			XPathNodeIterator	xpTranscripts  = null;
			XPathNodeIterator	xpLinks  = null;
			CXmlDesignation		xmlDesignation = null;
			string				strXPath = "";
			
			try
			{
				//	Free the existing collection
				m_xmlDesignations.Clear();
				
				//	Construct the path to use for the XPath query
				if((strScriptPath != null) && (strScriptPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strScriptPath, CXmlDesignation.XML_DESIGNATION_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}", CXmlDesignation.XML_DESIGNATION_ROOT_NAME, CXmlDesignation.XML_DESIGNATION_ELEMENT_NAME);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				
				//	Add an object for each designation
				while(xpIterator.MoveNext() == true)
				{
					xmlDesignation = new CXmlDesignation();
					m_tmaxEventSource.Attach(xmlDesignation.EventSource);
					
					//	Set the file information
					xmlDesignation.SetFileProps(m_strFileSpec);
					
					if(xmlDesignation.SetProperties(xpIterator.Current) == true)
					{
						//	Get the transcript text for this designation
						if((xpTranscripts = xpIterator.Current.Select(CXmlTranscript.XML_TRANSCRIPT_ELEMENT_NAME)) != null)
							xmlDesignation.GetTranscripts(xpTranscripts);

						//	Get the links for this designation
						if((xpLinks = xpIterator.Current.Select(CXmlLink.XML_LINK_ELEMENT_NAME)) != null)
							xmlDesignation.GetLinks(xpLinks);

						//	Add to the collection
						m_xmlDesignations.Add(xmlDesignation);
						
					}// if(xmlDesignation.SetProperties(xpIterator.Current) == true)
						
				}// while(xpIterator.MoveNext() == true)

				//	Make sure the designations are assigned a sort order value
				if(m_xmlDesignations.Count > 0)
					SetSortOrders();
					
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetDesignations", m_tmaxErrorBuilder.Message(ERROR_GET_DESIGNATIONS_EX, this.GetFileSpec()), Ex);
			}
			
			return bSuccessful;
			
		}// private bool GetDesignations(XPathDocument xpDocument, string strScriptPath)
		
		/// <summary>Called to get the XML scenes for this script</summary>
		/// <param name="xpDocument">The XPath document containing the source deposition</param>
		/// <param name="strScriptPath">The path to the parent script element</param>
		/// <returns>true if successful</returns>
		private bool GetScenes(XPathDocument xpDocument, string strScriptPath)
		{
			bool				bSuccessful = false;
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			CXmlScene			xmlScene = null;
			string				strXPath = "";
			
			try
			{
				//	Free the existing collection
				m_xmlScenes.Clear();
				
				//	Construct the path to use for the XPath query
				if((strScriptPath != null) && (strScriptPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strScriptPath, CXmlScene.XML_SCENE_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}", CXmlScene.XML_SCENE_ROOT_NAME, CXmlScene.XML_SCENE_ELEMENT_NAME);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				
				//	Add an object for each scene
				while(xpIterator.MoveNext() == true)
				{
					xmlScene = new CXmlScene();
					
					if(xmlScene.SetProperties(xpIterator.Current) == true)
					{
						//	Add to the collection
						m_xmlScenes.Add(xmlScene);
						
					}// if(xmlScene.SetProperties(xpIterator.Current) == true)
						
				}// while(xpIterator.MoveNext() == true)

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetScenes", m_tmaxErrorBuilder.Message(ERROR_GET_SCENES_EX, this.GetFileSpec()), Ex);
			}
			
			return bSuccessful;
			
		}// private bool GetScenes(XPathDocument xpDocument, string strScriptPath)
		
		/// <summary>Called to get the XML designations for this script</summary>
		/// <param name="xpDocument">The XPath document containing the source deposition</param>
		/// <param name="strParentPath">The path to the parent element</param>
		/// <returns>true if successful</returns>
		private bool GetDepositions(XPathDocument xpDocument, string strParentPath)
		{
			bool				bSuccessful = false;
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			XPathNodeIterator	xpSegments  = null;
			XPathNodeIterator	xpObjections  = null;
			CXmlDeposition		xmlDeposition = null;
			string				strXPath = "";
			
			try
			{
				//	Free the existing collection
				m_xmlDepositions.Clear();
				
				//	Construct the path to use for the XPath query
				if((strParentPath != null) && (strParentPath.Length > 0))
					strXPath = String.Format("{0}/{1}", strParentPath, CXmlDeposition.XML_DEPOSITION_ELEMENT_NAME);
				else
					strXPath = String.Format("{0}/{1}", CXmlDeposition.XML_DEPOSITION_ROOT_NAME, CXmlDeposition.XML_DEPOSITION_ELEMENT_NAME);
				
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select(strXPath)) == null) return false;
				
				//	Add an object for each deposition
				while(xpIterator.MoveNext() == true)
				{
					xmlDeposition = new CXmlDeposition();
					m_tmaxEventSource.Attach(xmlDeposition.EventSource);
					
					//	Set the file information
					xmlDeposition.SetFileProps(m_strFileSpec);
					
					if(xmlDeposition.SetProperties(xpIterator.Current) == true)
					{
						//	Get the segments for this deposition
						if((xpSegments = xpIterator.Current.Select(CXmlSegment.XML_SEGMENT_ELEMENT_NAME)) != null)
							xmlDeposition.GetSegments(xpSegments, true);

						//	Get any objections stored in the file
						if((xpObjections = xpIterator.Current.Select(CXmlObjections.XML_OBJECTIONS_ELEMENT_NAME)) != null)
							xmlDeposition.GetObjections(xpObjections);
						
						//	Add to the collection
						m_xmlDepositions.Add(xmlDeposition);
					
					}// if(xmlDeposition.SetProperties(xpIterator.Current) == true)
						
				}// while(xpIterator.MoveNext() == true)

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetDepositions", m_tmaxErrorBuilder.Message(ERROR_GET_DEPOSITIONS_EX, this.GetFileSpec()), Ex);
			}
			
			return bSuccessful;
			
		}// private bool GetDepositions(XPathDocument xpDocument, string strScriptPath)
		
		/// <summary>Called to get the playback time for all designations in the script</summary>
		/// <returns>The total playback time for all designations</returns>
		public double GetDuration()
		{
			if(m_xmlDesignations != null)
				return m_xmlDesignations.GetDuration();
			else
				return 0;
		
		}// public double GetDuration()
		
		/// <summary>This method is called to get the next available sort order value</summary>
		/// <returns>The next available sort order value</returns>
		private int GetNextSortOrder()
		{
			int iSortOrder = -1;
			
			try
			{
				if((m_xmlDesignations != null) && (m_xmlDesignations.Count > 0))
				{
					foreach(CXmlDesignation O in m_xmlDesignations)
					{
						if(O.XmlSortOrder >= iSortOrder)
							iSortOrder = O.XmlSortOrder + 1;
					}
					
				}
				else
				{
					iSortOrder = 1;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "GetNextSortOrder", Ex);
				iSortOrder = -2; // Just to make it easy to recognize
			}
			
			return iSortOrder;
					
		}// private int GetNextSortOrder()
		
		/// <summary>This method is called to set the SortOrder value for all designations in the collection</summary>
		private void SetSortOrders()
		{
			int iSortOrder = 1;
			
			try
			{
				if((m_xmlDesignations != null) && (m_xmlDesignations.Count > 0))
				{
					foreach(CXmlDesignation O in m_xmlDesignations)
					{
						O.XmlSortOrder = iSortOrder;
						iSortOrder++;
					}
					
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetSortOrders", m_tmaxErrorBuilder.Message(ERROR_SET_SORT_ORDERS_EX), Ex);
			}
					
		}// private void SetSortOrders()
		
		/// <summary>Called to edit the extents of the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be edited</param>
		/// <param name="lStartPL">The new start position</param>
		/// <param name="lStopPL">The new stop position</param>
		/// <param name="xmlAdded">Collection to store new designations resulting from the operation</param>
		/// <returns>true if successful</returns>
		private bool EditExtents(CXmlDesignation xmlDesignation, long lStartPL, long lStopPL, CXmlDesignations xmlAdded)
		{
			CXmlDesignations	xmlDesignations = null;
			CXmlDesignation		xmlMatch = null;
			bool				bSuccessful = false;
			bool				bBefore = true;
			
			Debug.Assert(xmlDesignation != null);
			if(xmlDesignation == null) return false;
			Debug.Assert(this.XmlDeposition != null);
			if(this.XmlDeposition == null) return false;
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			Debug.Assert(m_xmlDesignations.Contains(xmlDesignation) == true);
			if(m_xmlDesignations.Contains(xmlDesignation) == false) return false;
			
			try
			{
				//	Do we need to modify the extents?
				if((xmlDesignation.FirstPL != lStartPL) || (xmlDesignation.LastPL != lStopPL))
				{
					//	Create designations to represent the new range
					xmlDesignations = new CXmlDesignations();
					if(XmlDeposition.CreateDesignations(xmlDesignations, lStartPL, lStopPL, xmlDesignation.Highlighter) == false)
						return false;
						
					Debug.Assert(xmlDesignations.Count > 0);
					if(xmlDesignations.Count == 0) return false;
					
					//	Locate the designation with a matching segment identifier
					if((xmlMatch = xmlDesignations.FindFromSegment(xmlDesignation.Segment)) != null)
					{
						//	Adjust the caller's designation to match the new extents
						xmlDesignation.EditExtents(xmlMatch);
						xmlDesignation.SetNameFromExtents(this.XmlDeposition);
						
						//	Put any new designations into the script
						foreach(CXmlDesignation O in xmlDesignations)
						{
							//	Is this the matching designation?
							if(ReferenceEquals(O, xmlMatch) == true)
							{
								//	Switch the insertion direction
								bBefore = false;
							}
							else
							{
								//	Insert this designation into the script
								Insert(O, xmlDesignation, bBefore);
								
								//	Add to the caller's collection
								if(xmlAdded != null)
									xmlAdded.Add(O);
							
							}// if(ReferenceEquals(O, xmlMatch) == true)
							
						}// foreach(CXmlDesignation O in xmlDesignations)
							
					}
					else
					{
						//	New range puts us in a whole new segment so we copy the properties
						//	of the first new designation to our caller's designation
						xmlDesignation.Copy(xmlDesignations[0], true, true);
						
						//	Remove the first now that we've made a copy
						xmlDesignations.RemoveAt(0);
						
						//	Do we have designations to be added?
						if(xmlDesignations.Count > 0)
						{
							Insert(xmlDesignations, xmlDesignation, false);
							
							//	Add to the caller's collection
							if(xmlAdded != null)
								xmlAdded.AddRange(xmlDesignations);
						}
						
					}// if((xmlMatch = xmlDesignations.FindFromSegment(xmlDesignation.Segment)) != null)
					
				}// if((xmlDesignation.StartPL != lStartPL) || (xmlDesignation.StopPL != lStopPL))
				
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "EditExtents", m_tmaxErrorBuilder.Message(ERROR_EDIT_EXTENTS_EX, xmlDesignation.Name, lStartPL, lStopPL), Ex);
			}
			
			return bSuccessful;
			
		}// private bool EditExtents(CXmlDesignation xmlDesignation, long lStartPL, long StopPL, CXmlDesignations xmlAdded)
		
		/// <summary>Called to exclude the text in the specified designation</summary>
		/// <param name="xmlDesignation">The designation to be edited</param>
		/// <param name="lStartPL">The start position of the text to be excluded</param>
		/// <param name="lStopPL">The stop position of the text to be excluded</param>
		/// <param name="xmlAdded">Collection to store new designations resulting from the operation</param>
		/// <returns>true if successful</returns>
		private bool Exclude(CXmlDesignation xmlDesignation, long lStartPL, long lStopPL, CXmlDesignations xmlAdded)
		{
			
			CXmlDesignation	xmlSplit = null;
			bool			bSuccessful = false;

			Debug.Assert(xmlDesignation != null);
			if(xmlDesignation == null) return false;
			Debug.Assert(this.XmlDeposition != null);
			if(this.XmlDeposition == null) return false;
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			Debug.Assert(m_xmlDesignations.Contains(xmlDesignation) == true);
			if(m_xmlDesignations.Contains(xmlDesignation) == false) return false;
			
			try
			{
				//	Are we chopping off the start of the designation?
				if(lStartPL <= xmlDesignation.FirstPL)
				{
					//	Make sure the stop position is valid
					if(lStopPL < xmlDesignation.LastPL)
					{
						//	Split off the leading portion
						xmlSplit = xmlDesignation.Split(lStopPL, true, this.XmlDeposition);
					
						//	Now adjust the caller's designation to match the split off portion
						if(xmlSplit != null)
							xmlDesignation.Copy(xmlSplit);
					}
					else
					{
						//	Can't exclude the entire designation
						return false;
					}
					
				}
				else
				{
					//	Split off the trailing portion
					xmlSplit = xmlDesignation.Split(lStartPL, false, this.XmlDeposition);

					//	Are we cutting a piece out of the middle?
					if(lStopPL < xmlSplit.LastPL)
					{
						//	Split off the leading portion
						xmlSplit = xmlSplit.Split(lStopPL, true, this.XmlDeposition);

						if(xmlSplit != null)
						{
							Insert(xmlSplit, xmlDesignation, false);
				
							if(xmlAdded != null)
								xmlAdded.Add(xmlSplit);

						}

					}// if(lStopPL < xmlSplit.LastPL)
					
				}
						
				bSuccessful = true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Exclude", m_tmaxErrorBuilder.Message(ERROR_EXCLUDE_EX, xmlDesignation.Name, lStartPL, lStopPL), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Exclude(CXmlDesignation xmlDesignation, long lStartPL, long StopPL, CXmlDesignations xmlAdded)
		
		/// <summary>Called to split the specified designation based on the selection specified by the caller</summary>
		/// <param name="bBefore">True to split before the selection range</param>
		/// <param name="xmlDesignation">The designation to be split</param>
		/// <param name="lStartPL">The start position of the selected text</param>
		/// <param name="lStopPL">The stop position of the selected text</param>
		/// <param name="xmlAdded">Collection to store new designations resulting from the operation</param>
		/// <returns>true if successful</returns>
		private bool Split(CXmlDesignation xmlDesignation, bool bBefore, long lStartPL, long lStopPL, CXmlDesignations xmlAdded)
		{
			CXmlDesignation	xmlSplit = null;
			bool			bSuccessful = false;

			Debug.Assert(xmlDesignation != null);
			if(xmlDesignation == null) return false;
			Debug.Assert(this.XmlDeposition != null);
			if(this.XmlDeposition == null) return false;
			Debug.Assert(m_xmlDesignations != null);
			if(m_xmlDesignations == null) return false;
			Debug.Assert(m_xmlDesignations.Contains(xmlDesignation) == true);
			if(m_xmlDesignations.Contains(xmlDesignation) == false) return false;
			
			try
			{
				//	Are we splitting before the specified selection range?
				if(bBefore == true)
				{
					//	Make sure the start position is valid
					if(lStartPL > xmlDesignation.FirstPL)
					{
						//	Split off the portion from the start line to the end
						xmlSplit = xmlDesignation.Split(lStartPL, false, this.XmlDeposition);
					}
					else
					{
						//	Nothing to split
						return false;
					}
					
				}
				else
				{
					//	Is the stop position valid?
					if(lStopPL < xmlDesignation.LastPL)
					{
						//	Split off the leading portion
						xmlSplit = xmlDesignation.Split(lStopPL, true, this.XmlDeposition);
					}
					else
					{
						//	Nothing to split
						return false;
					}
					
				}
						
				//	Put the new designation into the script
				if(xmlSplit != null)
				{
					Insert(xmlSplit, xmlDesignation, false);
				
					if(xmlAdded != null)
						xmlAdded.Add(xmlSplit);

					bSuccessful = true;
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Split", m_tmaxErrorBuilder.Message(ERROR_SPLIT_EX, xmlDesignation.Name, lStartPL, lStopPL), Ex);
			}
			
			return bSuccessful;
			
		}// private bool Split(CXmlDesignation xmlDesignation, bool bBefore, long lStartPL, long StopPL, CXmlDesignations xmlAdded)
		
		/// <summary>This method validates the start and stop positions against the source deposition</summary>
		/// <param name="lStartPL">The start position to be validated</param>
		/// <param name="lStopPL">The stop position to be validated</param>
		/// <returns>true if the positions are valid</returns>
		private bool CheckRange(long lStartPL, long lStopPL)
		{
			bool	bSuccessful = false;
			long	lMinPL = 0;
			long	lMaxPL = 0;
			
			Debug.Assert(this.XmlDeposition != null);
			if(this.XmlDeposition == null) return false;
			
			try
			{
				//	Get the extents for the deposition
				lMinPL = this.XmlDeposition.GetFirstPL();
				lMaxPL = this.XmlDeposition.GetLastPL();
				
				//	Make sure the start position is valid
				if(lStartPL < lMinPL)
					lStartPL = lMinPL;
					
				//	Make sure the stop position is valid
				if(lStopPL > lMaxPL)
					lStopPL = lMaxPL;
					
				//	Make sure the range is not reversed
				if(lStopPL < lStartPL)
				{
					m_tmaxEventSource.FireError(this, "CheckRange", m_tmaxErrorBuilder.Message(ERROR_INVALID_EDIT_RANGE, lStartPL, lStopPL));
					return false;
				}
				
				bSuccessful = true;	
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "CheckRange", m_tmaxErrorBuilder.Message(ERROR_CHECK_RANGE_EX, lStartPL, lStopPL), Ex);
			}
			
			return bSuccessful;
				
		}// private bool CheckRange(long lStartPL, long lStopPL)
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>Enumerated script format identifier</summary>
		public TmaxXmlScriptFormats XmlScriptFormat
		{
			get{ return m_eXmlScriptFormat; }
			set{ m_eXmlScriptFormat = value; }
		}
		
		/// <summary>MediaId assigned to the script</summary>
		public string MediaId
		{
			get{ return m_strMediaId; }
			set{ m_strMediaId = value; }
		}
		
		/// <summary>Name assigned to the script</summary>
		public string Name
		{
			get{ return m_strName; }
			set{ m_strName = value; }
		}
		
		/// <summary>Flag to indicate if the file has been saved</summary>
		public bool Saved
		{
			get{ return m_bSaved; }
			set{ m_bSaved = value; }
		}
		
	/// <summary>Path to the source deposition</summary>
		public string SourceFileSpec
		{
			get{ return m_strSourceFileSpec; }
			set{ m_strSourceFileSpec = value; }
		}
		
		/// <summary>The XML depositions from which this script's designations are created</summary>
		public CXmlDepositions XmlDepositions
		{
			get{ return m_xmlDepositions; }
		}
		
		/// <summary>The XML deposition from which this script's designations are created</summary>
		public CXmlDeposition XmlDeposition
		{
			get{ return GetXmlDeposition(); }
			set{ SetXmlDeposition(value); }
		}
		
		/// <summary>The XML designations owned by the script</summary>
		public CXmlDesignations XmlDesignations
		{
			get{ return m_xmlDesignations; }
		}
		
		/// <summary>The XML scenes owned by the script</summary>
		public CXmlScenes XmlScenes
		{
			get{ return m_xmlScenes; }
		}
		
		#endregion Properties
		
	}// public class CXmlScript

	/// <summary>Objects of this class are used to manage a dynamic array of CXmlScript objects</summary>
	public class CXmlScripts : CTmaxSortedArrayList
	{
		#region Constants
		
		public const string XML_ERRORS_ELEMENT_NAME = "errors";

		#endregion Constants
		
		#region Public Members
		
		/// <summary>Default constructor</summary>
		public CXmlScripts() : base()
		{
			this.KeepSorted = false;
		}

		/// <summary>This method allows the caller to add an object to the list</summary>
		/// <param name="xmlError">CXmlScript object to be added to the list</param>
		/// <returns>The object just added if successful, null otherwise</returns>
		public CXmlScript Add(CXmlScript xmlError)
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
			
		}// Add(CXmlScript xmlError)

		/// <summary>This method is called to remove the requested object from the collection</summary>
		/// <param name="xmlError">The object to be removed</param>
		public void Remove(CXmlScript xmlError)
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
		public bool Contains(CXmlScript xmlError)
		{
			// Use base class to process actual collection operation
			return base.Contains(xmlError as object);
		}

		/// <summary>Overloaded version of [] operator to return the object at the desired index</summary>
		/// <returns>The object at the specified index</returns>
		public new CXmlScript this[int index]
		{
			// Use base class to process actual collection operation
			get 
			{ 
				return (GetAt(index) as CXmlScript);
			}
		}

		/// <summary>This method is called to retrieve the index of the specified object</summary>
		/// <param name="value">Object to be located</param>
		/// <returns>The zero-based index of the specified object</returns>
		public int IndexOf(CXmlScript value)
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
					foreach(CXmlScript O in this)
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
		
		#endregion Public Methods
		
	}//	public class CXmlScripts
		
}// namespace FTI.Shared.Xml
