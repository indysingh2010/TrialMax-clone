using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition node</summary>
	public class CXmlLoadFile : CXmlFile
	{
		#region Constants
		
		public const string XML_LOADFILE_VERSION = "1.00";

		public const string XML_LOADFILE_ROOT_NAME = "trialMax";
		public const string XML_LOADFILE_ELEMENT_NAME = "loadfile";

		public const string XML_LOADFILE_ATTRIBUTE_VERSION = "version";
		
		protected const int ERROR_OPEN_LOADFILE_EX		= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_LOADFILE_NODE		= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX		= (LAST_XML_FILE_ERROR + 3);
		protected const int	ERROR_OPEN_XPATH_EX			= (LAST_XML_FILE_ERROR + 4);
		protected const int	ERROR_GET_PRIMARIES_EX		= (LAST_XML_FILE_ERROR + 5);
		protected const int	ERROR_GET_ERRORS_EX			= (LAST_XML_FILE_ERROR + 6);
		protected const int	ERROR_GET_XPATH_DOCUMENT_EX	= (LAST_XML_FILE_ERROR + 7);
		protected const int	ERROR_OPEN_XML_READER_EX	= (LAST_XML_FILE_ERROR + 8);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Local member bound to Node property</summary>
		private XmlNode m_xmlNode = null;

		/// <summary>Local member bound to Primaries property</summary>
		private CXmlPrimaries m_xmlPrimaries = new CXmlPrimaries();

		/// <summary>Local member bound to Errors property</summary>
		private CXmlErrors m_xmlErrors = new CXmlErrors();

		/// <summary>This member is bounded to the Version property</summary>
		protected string m_strVersion = XML_LOADFILE_VERSION;		

		/// <summary>Local member to store reference to the active XPath document</summary>
		protected XPathDocument m_xpathDocument = null;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlLoadFile()
		{
			Initialize(null);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="xmlNode">The XML node used to create the object</param>
		public CXmlLoadFile(XmlNode xmlNode)
		{
			Initialize(xmlNode);
		}
		
		/// <summary>This method is called to close the XML file associated with the deposition</summary>
		/// <param name="bClear">true to clear the local collections</param>
		public void Close(bool bClear)
		{
			//	Do the base class processing
			base.Close();
			
			//	Should we clear the collections?
			if(bClear == true)
				Clear();
			
			m_xmlNode = null;
			if(m_xpathDocument != null)
				m_xpathDocument = null;
			
			// NOTE:	We do not clear the FileSpec value so that the user can set it
			//			once and then read / write at will
		}
			
		/// <summary>This method is called to get the default file extension</summary>
		/// <returns>The default extension</returns>
		static public string GetDefaultExtension()
		{
			return "xmlf";
		}
		
		/// <summary>Called to clear the depositions child collections</summary>
		public void Clear()
		{
			if(m_xmlErrors != null)
			{
				m_xmlErrors.Clear();
			}
			
			if(m_xmlPrimaries != null)
			{
				m_xmlPrimaries.Clear();
			}
			
		}// public void Clear()
		
		/// <summary>This method is called to open the specified file</summary>
		/// <param name="bCreate">true to create the file if it doesn't exist</param>
		/// <returns>true if successful</returns>
		public override bool Open(bool bCreate)
		{
			bool bSuccessful = false;
			
			//	Do the base class processing first
			if(base.Open(bCreate) == false) return false;
			
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlRoot != null);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Get the deposition descriptor node
					if((m_xmlNode = m_xmlRoot.SelectSingleNode(XML_LOADFILE_ELEMENT_NAME)) == null)
					{
						//	Is this a new file?
						if(bCreate == true)
						{
							//	We don't expect the load file node to 
							//	appear in a new file
							bSuccessful = true;
						}
						else
						{
							m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_LOADFILE_NODE, m_strFileSpec));
						}
						break;
					}
					
					//	Use the XML node to set the properties for this object
					if(SetProperties(m_xmlNode) == false)
						break;
						
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_LOADFILE_EX, m_strFileSpec), Ex);
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
		
		/// <summary>This method is called to populate the primaries collection</summary>
		/// <param name="bSecondaries">true to load the primary's secondary children</param>
		/// <returns>true if successful</returns>
		/// <remarks>This method assumes the object has been initialized with a call to Open()</remarks>
		public bool GetPrimaries(bool bSecondaries)
		{
			CXmlPrimary xmlPrimary = null;
			
			//	Try to use an XPath query if we don't have the <loadfile> node
			if(m_xmlNode == null)
			{
				GetFileSpec();
				if(m_strFileSpec.Length > 0)
				{
					if(GetXPathDocument(m_strFileSpec) != null)
						return GetPrimaries(m_xpathDocument, bSecondaries);
				}
				
				//	Must not have been able to use XPath
				return false;
			
			}// if(m_xmlNode == null)
			
			//	Clear the primaries collection
			Debug.Assert(m_xmlPrimaries != null);
			m_xmlPrimaries.Clear();
			
			//	Locate all the primary nodes
			foreach(XmlNode O in m_xmlNode.ChildNodes)
			{
				if(String.Compare(O.Name, "primary", true) != 0) continue;
				
				xmlPrimary = new CXmlPrimary(O);
				m_tmaxEventSource.Attach(xmlPrimary.EventSource);	
						
				if(xmlPrimary.SetProperties(O) == false) continue;

				//	Add to the collection
				m_xmlPrimaries.Add(xmlPrimary);
				
				//	Are we supposed to get the secondaries?
				if(bSecondaries == true)
				{
					xmlPrimary.GetSecondaries();
				}
				else
				{
					//	Assume all the children are secondaries
					xmlPrimary.Children = O.ChildNodes != null ? O.ChildNodes.Count : 0;
				}
			
			}// foreach(XmlNode O in m_xmlNode.ChildNodes)

			m_xmlPrimaries.Sort(true);
			return true;
			
		}// public bool GetPrimaries(bool bSecondaries)
			
		/// <summary>This method is called to populate the primaries collection</summary>
		/// <param name="xpDocument">The XPath document containing the primary nodes</param>
		/// <param name="bSecondaries">true to load the primary's secondary children</param>
		/// <returns>true if successful</returns>
		public bool GetPrimaries(XPathDocument xpDocument, bool bSecondaries)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			CXmlPrimary			xmlPrimary = null;
			
			try
			{
				//	Make sure we have a valid primaries collection
				Debug.Assert(m_xmlPrimaries != null);
				m_xmlPrimaries.Clear();
					
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select("trialMax/loadfile/primary")) == null) return false;

				//	Add an object for each primarie
				while(xpIterator.MoveNext() == true)
				{
					xmlPrimary = new CXmlPrimary();
					m_tmaxEventSource.Attach(xmlPrimary.EventSource);

					if(xmlPrimary.SetProperties(xpIterator.Current) == true)
					{
						m_xmlPrimaries.Add(xmlPrimary);
						
						//	Should we get the secondary children?
						if(bSecondaries == true)
							xmlPrimary.GetSecondaries(xpDocument);
					}
						
				}// while(xpIterator.MoveNext() == true)

				m_xmlPrimaries.Sort(true);
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetPrimaries", m_tmaxErrorBuilder.Message(ERROR_GET_PRIMARIES_EX), Ex);
				return false;
			}
			
		}// public bool GetPrimaries(XPathDocument xpDocument)
		
		/// <summary>This method is called to populate the primaries collection</summary>
		/// <param name="xmlReader">The XML text reader used to open the file</param>
		/// <param name="bSecondaries">true to load the primary's secondary children</param>
		/// <returns>true if successful</returns>
		public bool GetPrimaries(XmlTextReader xmlReader, bool bSecondaries)
		{
			CXmlPrimary	xmlPrimary = null;
			int			iDepth = 0;
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.EOF == true) return false;

			try
			{
				//	Make sure we have a valid primaries collection
				Debug.Assert(m_xmlPrimaries != null);
				m_xmlPrimaries.Clear();
					
				//	Set the depth where the primary nodes should be
				//
				//	NOTE:	We assume the reader is positioned on the parent
				//			<loadfile></loadfile> node
				iDepth = xmlReader.Depth;
				
				//	Start reading the primaries from the file
				while(AdvanceReader(xmlReader, false) == true)
				{
					//	Don't bother if this node is not an element
					if(xmlReader.NodeType != XmlNodeType.Element) continue;

					//	Is it time to break out?
					if(xmlReader.Depth <= iDepth)
						break;

					//	Is this a primary?
					if(xmlReader.Name == "primary")
					{
						xmlPrimary = new CXmlPrimary();
						m_tmaxEventSource.Attach(xmlPrimary.EventSource);
						
						if(xmlPrimary.SetProperties(xmlReader) == true)
						{
							m_xmlPrimaries.Add(xmlPrimary);
							
							//	Add the secondaries
							xmlPrimary.GetSecondaries(xmlReader, bSecondaries);
						}
					
					}// if(xmlReader.Name == "primary")

				}// while(AdvanceReader(xmlReader, false) == true)
				
				m_xmlPrimaries.Sort(true);
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetPrimaries", m_tmaxErrorBuilder.Message(ERROR_GET_PRIMARIES_EX), Ex);
				return false;
			}
			
		}// public bool GetPrimaries(XmlTextReader xmlReader)
		
		/// <summary>This method is called to populate the secondaries collection of the specified primary object</summary>
		/// <param name="xmlPrimary">The primary object to be populated</param>
		/// <returns>true if successful</returns>
		public bool GetSecondaries(CXmlPrimary xmlPrimary)
		{
			//	Have we retrieved the XML node for this primary?
			if(xmlPrimary.Node != null)
			{
				//	Let the object use its internal node
				return xmlPrimary.GetSecondaries();
			}
			else
			{
				//	Do we need to get an XPath document?
				if(m_xpathDocument == null)
				{
					//	Make sure we have the file path
					GetFileSpec();
					if(m_strFileSpec.Length == 0) return false;
					
					//	Try to get the XPath document
					if(GetXPathDocument(m_strFileSpec) == null) return false;

				}// if(m_xpathDocument == null)
				
				return xmlPrimary.GetSecondaries(m_xpathDocument);
			}
			
		}// public bool GetSecondaries(CXmlPrimary xmlPrimary)
			
		/// <summary>This method is called to populate the errors collection using the active document</summary>
		/// <returns>true if successful</returns>
		/// <remarks>This method assumes the XML document has been initialized with a call to Open()</remarks>
		public bool GetErrors()
		{
			XmlNodeList xmlErrors = null;
			CXmlError	xmlError = null;
			
			Debug.Assert(m_xmlDocument != null);
			Debug.Assert(m_xmlRoot != null);
			if(m_xmlDocument == null) return false;
			if(m_xmlRoot == null) return false;

			try
			{
				//	Make sure we have a errors collection
				Debug.Assert(m_xmlErrors != null);
				m_xmlErrors.Clear();
					
				if((xmlErrors = m_xmlRoot.SelectNodes("errors/error")) != null)
				{
					foreach(XmlNode O in xmlErrors)
					{
						xmlError = new CXmlError();
						m_tmaxEventSource.Attach(xmlError.EventSource);

						if(xmlError.SetProperties(O) == true)
						{
							m_xmlErrors.Add(xmlError);
						}
						
					}
				}

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetErrors", m_tmaxErrorBuilder.Message(ERROR_GET_ERRORS_EX), Ex);
			}
			
			return false;
			
		}// public bool GetErrors()
		
		/// <summary>This method is called to populate the errors collection</summary>
		/// <param name="xpDocument">The XPath document containing the primary nodes</param>
		/// <returns>true if successful</returns>
		public bool GetErrors(XPathDocument xpDocument)
		{
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;
			CXmlError			xmlError = null;
			
			try
			{
				//	Make sure we have a valid errors collection
				Debug.Assert(m_xmlErrors != null);
				m_xmlErrors.Clear();
					
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return false;
				if((xpIterator = xpNavigator.Select("trialMax/errors/error")) == null) return false;
				
				//	Add an object for each error
				while(xpIterator.MoveNext() == true)
				{
					xmlError = new CXmlError();
					xmlError.EventSource.ErrorEvent += new FTI.Shared.Trialmax.ErrorEventHandler(this.EventSource.OnError);
					xmlError.EventSource.DiagnosticEvent += new FTI.Shared.Trialmax.DiagnosticEventHandler(this.EventSource.OnDiagnostic);
					
					if(xmlError.SetProperties(xpIterator.Current) == true)
					{
						m_xmlErrors.Add(xmlError);
					}
						
				}// while(xpIterator.MoveNext() == true)

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetErrors", m_tmaxErrorBuilder.Message(ERROR_GET_ERRORS_EX), Ex);
				return false;
			}
			
		}// public bool GetErrors(XPathDocument xpDocument)
		
		/// <summary>This method is called to populate the errors collection</summary>
		/// <param name="xmlReader">The XML text reader used to open the file</param>
		/// <returns>true if successful</returns>
		public bool GetErrors(XmlTextReader xmlReader)
		{
			CXmlError	xmlError = null;
			int			iDepth = 0;
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.EOF == true) return false;
			
			try
			{
				//	Set the depth where the error nodes should be
				//
				//	NOTE:	We assume the reader is positioned on the parent
				//			<errors></errors> node
				iDepth = xmlReader.Depth;
				
				//	Make sure we have a valid errors collection
				Debug.Assert(m_xmlErrors != null);
				m_xmlErrors.Clear();
					
				//	Start reading the errors from the file
				while(xmlReader.Read() == true)
				{
					//	Is it time to break out?
					if(xmlReader.Depth <= iDepth)
						break;

					//	Is this an error?
					if(xmlReader.Name == "error")
					{
						xmlError = new CXmlError();
						m_tmaxEventSource.Attach(xmlError.EventSource);
						
						if(xmlError.SetProperties(xmlReader) == true)
							m_xmlErrors.Add(xmlError);
						else
							xmlError = null;
					}

				}// while(xmlReader.Read() == true)
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetErrors", m_tmaxErrorBuilder.Message(ERROR_GET_ERRORS_EX), Ex);
				return false;
			}
			
		}// public bool GetErrors(XmlTextReader xmlReader)
		
		/// <summary>This method opens the file and fills the collection using XPath queries</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="bPrimaries">true to fill the Primaries collection</param>
		/// <param name="bSecondaries">true to fill the secondaries collection of each primary node</param>
		/// <param name="bErrors">true to fill the errors collection</param>
		/// <returns>true if successful</returns>
		public bool OpenXPath(string strFileSpec, bool bPrimaries, bool bSecondaries, bool bErrors)
		{
			try
			{
				//	Make sure we have an XPath document
				if(GetXPathDocument(strFileSpec) == null)
					return false;
			
				//	Get the Primary nodes if requested
				if(bPrimaries == true)
					GetPrimaries(m_xpathDocument, bSecondaries);
				
				//	Get the errors
				if(bErrors == true)
					GetErrors(m_xpathDocument);

				return true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenXPath", m_tmaxErrorBuilder.Message(ERROR_OPEN_XPATH_EX, strFileSpec), Ex);
			}
			
			//	Must have been a problem
			return false;
			
		}// public bool OpenXPath(string strFileSpec, bool bPrimaries, bool bSecondaries, bool bErrors)
		
		/// <summary>This method opens the file and fills the collection using XPath queries</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <returns>true if successful</returns>
		public bool OpenXPath(string strFileSpec)
		{
			return OpenXPath(strFileSpec, true, true, true);
		}

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
				strElementName = XML_LOADFILE_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_LOADFILE_ATTRIBUTE_VERSION, XML_LOADFILE_VERSION) == false)
						break;
						
					if((Primaries != null) && (Primaries.Count > 0))
					{
						foreach(CXmlPrimary O in Primaries)
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
		
		/// <summary>This method is called to save the deposition to file</summary>
		/// <returns>The Xml writer object if successful</returns>
		public override bool Save()
		{
			XmlTextWriter	xmlWriter = null;
			string			strBackUp = "";
			XmlNode			xmlChild = null;
			
			//	Construct the full path specification
			GetFileSpec();
			if(m_strFileSpec.Length == 0) return false;
			
			//	Should we back up the existing file?
			if(System.IO.File.Exists(m_strFileSpec) == true)
			{
				strBackUp = System.IO.Path.ChangeExtension(m_strFileSpec, "_xml_");
				try
				{
					if(System.IO.File.Exists(strBackUp) == true)
						System.IO.File.Delete(strBackUp);
						
					System.IO.File.Move(m_strFileSpec, strBackUp);
				}
				catch
				{
					strBackUp = "";
				}
			}
			
			try
			{
				//	Make sure the comments array has been populated
				SetComments();
				
				//	Open the file
				if(Open(true) == true)
				{
					Debug.Assert(m_xmlDocument != null);
					Debug.Assert(m_xmlRoot != null);

					//	Get the top-level node for the document
					if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					{
						m_xmlRoot.AppendChild(xmlChild);
						
						//	Because the errors  nodes are not
						//	parented by the loadfile node we have to add them
						//	here
						if(m_xmlErrors != null)
						{
							if((xmlChild = m_xmlErrors.ToXmlNode(m_xmlDocument, null)) != null)
								m_xmlRoot.AppendChild(xmlChild);
						}
						
						if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
						{
							xmlWriter.Formatting = System.Xml.Formatting.Indented;
							xmlWriter.Indentation = 4;
							
							m_xmlDocument.Save(xmlWriter);
							xmlWriter.Close();
							
							//	Close the document without destroying the local collections
							Close(false);
							
							//	Delete the backup
							if(strBackUp.Length > 0)
							{
								try { System.IO.File.Delete(strBackUp); }
								catch { }
							}
							
							return true;
						}
						
					}// if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
			}
			
			//	Restore the backup
			if(strBackUp.Length > 0)
			{
				try
				{
					if(System.IO.File.Exists(m_strFileSpec) == true)
						System.IO.File.Delete(m_strFileSpec);
						
					System.IO.File.Move(strBackUp, m_strFileSpec);
				}
				catch
				{
				}
					
			}

			return false;
		
		}//	Save()
		
		/// <summary>This method is called to fill the collections using an XML text reader for best performance</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <param name="bPrimaries">true to fill the Primaries collection</param>
		/// <param name="bSecondaries">true to fill the secondaries collection of each primary node</param>
		/// <param name="bErrors">true to fill the errors collection</param>
		/// <returns>true if successful</returns>
		public bool OpenXmlReader(string strFileSpec, bool bPrimaries, bool bSecondaries, bool bErrors)
		{
			XmlTextReader	xmlReader = null;
			bool			bSuccessful = false;
			bool			bSkip = false;

			//	Make sure the file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "OpenXmlReader", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, strFileSpec));
				return false;
			}
			
			try
			{
				//	Load the file
				xmlReader = new System.Xml.XmlTextReader(strFileSpec);
				
				//	Line up on the first content node
				xmlReader.WhitespaceHandling = WhitespaceHandling.None;
				xmlReader.MoveToContent();
				
				//	Store the file specification
				SetFileProps(strFileSpec);
				m_strFileSpec = strFileSpec;
				
				//	Read the file
				while(AdvanceReader(xmlReader, bSkip) == true)
				{
					//	Clear the skip flag
					bSkip = false;
					
					//	Don't bother if not an element
					if(xmlReader.NodeType != XmlNodeType.Element) continue; 
					
					//	Is this the <loadfile></loadfile> element?
					if(xmlReader.Name == "loadfile")
					{
						//	Use the reader to set the load file properties
						SetProperties(xmlReader);
						
						//	Are we supposed to get the primaries?
						if(bPrimaries == true)
						{
							GetPrimaries(xmlReader, bSecondaries);
						}
						else
						{
							//	Skip processing the child primaries
							bSkip = true;
							continue;
						}
						
						//	Stop here if not retrieving Errors
						if(bErrors == false)
							break;
						
					}// if(xmlReader.Name == "loadfile")
					
					//	Is this the Errors?
					//
					//	NOTE:	We do not use "else if" because the reader may
					//			have been left on the Errors node after reading
					//			in the primaries
					if(xmlReader.Name == "errors")
					{
						if(bErrors == true)
						{
							GetErrors(xmlReader);
							
							if(xmlReader.EOF == true)
								break;
						}
						else
						{
							//	Skip over the child errors
							bSkip = true;
							continue;
						}
						
					}// if(xmlReader.Name == "errors")

				}// while(AdvanceReader(xmlReader, bSkip) == true)

				bSuccessful = true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "OpenXmlReader", m_tmaxErrorBuilder.Message(ERROR_OPEN_XML_READER_EX, strFileSpec), Ex);
			}
			finally
			{
				if(xmlReader != null)
				{
					xmlReader.Close();
					xmlReader = null;
				}
				
			}
			
			return bSuccessful;
			
		}// public bool OpenXmlReader(string strFileSpec, bool bPrimaries, bool bSecondaries, bool bErrors)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>Initializes the object when created</summary>
		/// <param name="xmlNode">The node being used to create the object</param>
		protected void Initialize(XmlNode xmlNode)
		{
			m_xmlNode = xmlNode;
			
			m_strRoot = XML_LOADFILE_ROOT_NAME;
			Extension = GetDefaultExtension();
			
			m_xmlPrimaries.Comparer = new CXmlBaseSorter();
			m_xmlPrimaries.KeepSorted = false;

			m_xmlErrors.Comparer = new CXmlBaseSorter();
			m_xmlErrors.KeepSorted = false;

			//	Populate the error builder
			SetErrorStrings();
		
		}// protected void Initialize(XmlNode xmlNode)
		
		/// <summary>This method uses the current document to set the deposition properties</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		protected bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			if(xpNavigator == null) return false;

			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_LOADFILE_ATTRIBUTE_VERSION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strVersion = strAttribute;
				
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// protected bool SetProperties()
		
		/// <summary>This method uses the specified node to set the deposition properties</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		protected bool SetProperties(XmlNode xmlNode)
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
			
		}// protected bool SetProperties()
		
		/// <summary>This method uses the current document to set the deposition properties</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		protected bool SetProperties(XmlTextReader xmlReader)
		{
			string strAttribute = "";
			
			Debug.Assert(xmlReader != null);
			if(xmlReader == null) return false;
			if(xmlReader.HasAttributes == false) return false;
			
			try
			{
				strAttribute = xmlReader.GetAttribute(XML_LOADFILE_ATTRIBUTE_VERSION,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strVersion = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// protected bool SetProperties()
		
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
			aStrings.Add("An exception was raised while attempting to open the load file: filename = %1");
			aStrings.Add("%1 is not a valid XML load file. It does not contain the top-level loadfile node.");
			aStrings.Add("An exception was raised while attempting to set the load file properties");
			aStrings.Add("An exception was raised while attempting to open the file with an XPath query: filename = %1");
			aStrings.Add("An exception was raised while attempting to retrieve the primary nodes");
			aStrings.Add("An exception was raised while attempting to retrieve the error nodes");
			aStrings.Add("An exception was raised while attempting to open an XPath document: filename = %1");
			aStrings.Add("An exception was raised while attempting to load the file with an XML text reader: filename = %1");
		}
		
		/// <summary>This method is called to populate the comments array prior to saving the file</summary>
		protected override void SetComments()
		{
			m_aComments.Clear();
			m_aComments.Add("TrialMax .NET XML Load File");
			m_aComments.Add("Revision " + Version);
			m_aComments.Add("Copyright FTI Consulting");	
		
		}// protected override void SetComments()
		
		/// <summary>This method is called to get an XPath document for the specified file</summary>
		/// <param name="strFileSpec">The fully qualified path to the XML file</param>
		/// <returns>The XPath document if successful</returns>
		protected XPathDocument GetXPathDocument(string strFileSpec)
		{
			XPathDocument		xpDocument = null;
			XPathNavigator		xpNavigator = null;
			XPathNodeIterator	xpIterator  = null;

			//	Do we already have an XPath document?
			if(m_xpathDocument != null)
			{
				//	Do the files match?
				if(String.Compare(strFileSpec, this.FileSpec, true) == 0)
				{
					return m_xpathDocument;
				}
				
			}// if(m_xpathDocument != null)
			
			//	Make sure the specified file exists
			if(System.IO.File.Exists(strFileSpec) == false)
			{
				m_tmaxEventSource.FireError(this, "GetXPathDocument", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, strFileSpec));
				return null;
			}
			
			try
			{
				xpDocument = new System.Xml.XPath.XPathDocument(strFileSpec);
				
				//	Get the load file node
				if((xpNavigator = xpDocument.CreateNavigator()) == null) return null;
				if((xpIterator = xpNavigator.Select("trialMax/loadfile")) == null) return null;

				if(xpIterator.Count > 0)
				{
					xpIterator.MoveNext();
					SetProperties(xpIterator.Current);
				}
				else
				{
					m_tmaxEventSource.FireError(this, "GetXPathDocument", m_tmaxErrorBuilder.Message(ERROR_NO_LOADFILE_NODE, strFileSpec));
					return null;
				}

				//	Update the path information
				SetFileProps(strFileSpec);
				m_strFileSpec = strFileSpec;
				
				//	Reset the active reference
				m_xpathDocument = xpDocument;
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "GetXPathDocument", m_tmaxErrorBuilder.Message(ERROR_GET_XPATH_DOCUMENT_EX, strFileSpec), Ex);
			}
			
			return m_xpathDocument;
			
		}// protected XPathDocument GetXPathDocument(string strFileSpec)
		
		#endregion Protected Methods
		
		#region Properties
		
		//	TrialMax load file version descriptor
		public string Version
		{
			get{ return m_strVersion; }
			set{ m_strVersion = value; }
			
		}// Version Property
		
		//	The XML node associated with this object
		public XmlNode Node
		{
			get{ return m_xmlNode; }
			
		}// Node Property
		
		//	Collection of primaries that belong to the load file
		public CXmlPrimaries Primaries
		{
			get{ return m_xmlPrimaries; }
			
		}// Primaries Property
		
		//	Collection of XML error nodes in the file
		public CXmlErrors Errors
		{
			get{ return m_xmlErrors; }
			
		}// Errors Property
		
		#endregion Properties
		
	}// public class CXmlLoadFile

}// namespace FTI.Shared.Xml
