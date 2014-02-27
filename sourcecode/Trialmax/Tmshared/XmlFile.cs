using System;
using System.Collections;
using System.Xml;
using System.Windows.Forms;

namespace FTI.Shared.Xml
{
	/// <summary>This class is used to create and manage an Xml file</summary>
	public class CXmlFile : CXmlBase
	{
		#region Constants
		
		protected const int ERROR_CREATE_FILE_EX	= (LAST_XML_BASE_ERROR + 1);
		protected const int ERROR_OPEN_FILE_EX		= (LAST_XML_BASE_ERROR + 2);
		protected const int ERROR_SAVE_FILE_EX		= (LAST_XML_BASE_ERROR + 3);
		protected const int ERROR_WRITE_FILE_EX		= (LAST_XML_BASE_ERROR + 4);
		protected const int ERROR_FILE_NOT_FOUND	= (LAST_XML_BASE_ERROR + 5);
		protected const int	ERROR_NO_ROOT_NODE		= (LAST_XML_BASE_ERROR + 6);
		
		protected const int LAST_XML_FILE_ERROR		= (LAST_XML_BASE_ERROR + 6);
		
		#endregion Constants
		
		#region Protected Members
		
		/// <summary>Local member accessed by the Document property</summary>
		protected XmlDocument m_xmlDocument = null;			
		
		/// <summary>Local member to store reference to the document's root node</summary>
		protected XmlNode m_xmlRoot = null;		
		
		/// <summary>Local member accessed by the AddDateToFilename property</summary>
		protected bool m_bAddDateToFilename = false;			
		
		/// <summary>Local member accessed by the Folder property</summary>
		protected string m_strFolder = "";			
		
		/// <summary>Local member accessed by the Filename property</summary>
		protected string m_strFilename = "";			
		
		/// <summary>Local member accessed by the Extension property</summary>
		protected string m_strExtension = ".xml";			
		
		/// <summary>Local member accessed by the FileSpec property</summary>
		protected string m_strFileSpec = "";			
		
		/// <summary>Local member accessed by the Root property</summary>
		protected string m_strRoot = "root";			
		
		/// <summary>Local member accessed by the Comments property</summary>
		protected ArrayList m_aComments = new ArrayList();	
		
		#endregion Protected Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlFile() : base()
		{
		}
		
		/// <summary>
		/// This method is called to add a new element to the file
		/// </summary>
		/// <param name="xmlObject">Interface to the node to be added</param>
		/// <param name="strName">Name to assign to the node</param>
		/// <param name="bSave">true to save the file after writing</param>
		/// <returns>true if successful</returns>
		public virtual bool Write(IXmlObject xmlObject, string strName, bool bSave)
		{
			XmlNode	xmlNode = null;
			XmlNode xmlRoot;
			
			try
			{
				//	Make sure the document is open
				if(m_xmlDocument == null)
				{
					if(Open() == false)
						return false;
				}
					
				if((xmlNode = xmlObject.ToXmlNode(m_xmlDocument, strName)) != null)
				{
					if((xmlRoot = m_xmlDocument.DocumentElement) != null)
					{
						xmlRoot.AppendChild(xmlNode);
					}
				}
		
				if(bSave == true)
					Save();
					
				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Write", m_tmaxErrorBuilder.Message(ERROR_WRITE_FILE_EX, m_strFileSpec), Ex);
			}
			
			return true;
		
		}//	public virtual bool Write(IXmlObject xmlObject, bool bSave)
		
		/// <summary>
		/// This method is called to add a new element to the file
		/// </summary>
		/// <param name="xmlObject">Interface to the node to be added</param>
		/// <param name="strName">Name to assign to the node</param>
		/// <returns>true if successful</returns>
		public virtual bool Write(IXmlObject xmlObject, string strName)
		{
			return Write(xmlObject, strName, true);
		
		}// public virtual bool Write(IXmlObject xmlObject)

		/// <summary>
		/// This method is called to add a new element to the file
		/// </summary>
		/// <param name="xmlObject">Interface to the node to be added</param>
		/// <param name="bSave">True to save</param>
		/// <returns>true if successful</returns>
		public virtual bool Write(IXmlObject xmlObject, bool bSave)
		{
			return Write(xmlObject, "", bSave);
		
		}// public virtual bool Write(IXmlObject xmlObject)

		/// <summary>
		/// This method is called to add a new element to the file
		/// </summary>
		/// <param name="xmlObject">Interface to the node to be added</param>
		/// <returns>true if successful</returns>
		public virtual bool Write(IXmlObject xmlObject)
		{
			return Write(xmlObject, "", true);
		
		}// public virtual bool Write(IXmlObject xmlObject)

		#endregion Public Methods
		
		#region Public Methods
		
		/// <summary>This method is called to get the filter string used to initialize a file selection form</summary>
		/// <param name="bAllowAll">true to allow All Files option</param>
		/// <returns>The appropriate filter string</returns>
		static public string GetFilter(bool bAllowAll)
		{
			if(bAllowAll == true)
				return "All Files (*.*)|*.*";
			else
				return "";
		}
		
		/// <summary>This method is called to get the default file extension</summary>
		/// <returns>The default file extension</returns>
		static public string GetExtension()
		{
			return "xml";
		}
		
		/// <summary>This method is called to parse the file spec and set the file properties</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		public virtual void SetFileProps(string strFileSpec)
		{
			string strText;
			
			if((strFileSpec != null) && (strFileSpec.Length > 0))
			{
				strText = System.IO.Path.GetDirectoryName(strFileSpec);
				if(strText.Length > 0) m_strFolder = strText;
				
				strText = System.IO.Path.GetFileNameWithoutExtension(strFileSpec);
				if(strText.Length > 0) m_strFilename = strText;
				
				strText = System.IO.Path.GetExtension(strFileSpec);
				if(strText.Length > 0) m_strExtension = strText;
			}
			else
			{
				m_strFolder = "";
				m_strFilename = "";
				m_strExtension = "";
			}
			
		}// public virtual void SetFileProps(string strFileSpec)

		/// <summary>This method is called to construct the full path specification of the log file</summary>
		protected virtual string GetFileSpec()
		{
			//	Set the folder
			m_strFileSpec = m_strFolder;
			if((m_strFileSpec.Length > 0) && (m_strFileSpec.EndsWith("\\") == false))
				m_strFileSpec += "\\";
			
			//	Add the filename
			m_strFileSpec += m_strFilename;
			
			//	Append the date if requested
			if(m_bAddDateToFilename)
				m_strFileSpec += (DateTime.Now.ToString("MM-dd-yyyy"));
				
			//	Now add the extension
			if(m_strExtension.Length > 0)
			{
				if((m_strFileSpec.EndsWith(".") == false) && (m_strExtension.StartsWith(".") == false))
				{
					m_strFileSpec += ".";
				}
				
				m_strFileSpec += m_strExtension;
			}
			
			return m_strFileSpec;
			
		}// protected virtual void GetFileSpec()

		/// <summary>This method is called to save the Xml document object</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		/// <returns>true if successful</returns>
		public virtual bool Save(string strFileSpec)
		{
			if((strFileSpec != null) && (strFileSpec.Length > 0))
				SetFileProps(strFileSpec);
			
			return Save();
		
		}// public virtual bool Save(string strFileSpec)

		/// <summary>This method is called to save the current document to file</summary>
		/// <returns>The Xml writer object if successful</returns>
		public virtual bool Save()
		{
			XmlTextWriter xmlWriter = null;
			
			//	Do we have a valid document?
			if(m_xmlDocument == null) return false;
			
			//	Construct the full path specification
			GetFileSpec();
			if(m_strFileSpec.Length == 0) return false;
			
			try
			{
				if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
				{
					xmlWriter.Formatting = Formatting.Indented;

					m_xmlDocument.Save(xmlWriter);
					xmlWriter.Close();
					return true;
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
			}
			
			return false;
		
		}//	public virtual bool Save()
		
		/// <summary>This method is called to get close the file</summary>
		public virtual void Close()
		{
			m_xmlDocument = null;
			m_xmlRoot = null;
		}
			
		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <returns>true if successful</returns>
		public virtual bool Open()
		{	
			return Open(m_strFileSpec, true);
		}
			
		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		/// <returns>true if successful</returns>
		public virtual bool Open(string strFileSpec)
		{
			return Open(strFileSpec, true);
		}
			
		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		/// <param name="bCreate">True to allow creation of the file</param>
		/// <returns>true if successful</returns>
		public virtual bool Open(string strFileSpec, bool bCreate)
		{
			if((strFileSpec != null) && (strFileSpec.Length > 0))
				SetFileProps(strFileSpec);
			
			return Open(bCreate);
		}

		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <param name="bCreate">True to allow creation of the file</param>
		/// <returns>true if successful</returns>
		public virtual bool Open(bool bCreate)
		{
			//	Close the existing document
			Close();
			
			//	Construct the path for the xml file
			GetFileSpec();
			
			//	Do we need to create the xml file?
			if(System.IO.File.Exists(m_strFileSpec) == false)
			{
				//	Should we create the file?
				if(bCreate == true)
				{
					//	Create a new file
					if(Create(m_strFileSpec) == false)
						return false;
				}
				else
				{
					m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_FILE_NOT_FOUND, m_strFileSpec));
					return false;
				}
			
			}// if(System.IO.File.Exists(m_strFileSpec) == false)
			
			try
			{
				//	Create the document object
				if((m_xmlDocument = new XmlDocument()) != null)
				{
					m_xmlDocument.Load(m_strFileSpec);
					
					//	Get the root node
					if((m_xmlRoot = m_xmlDocument.DocumentElement) == null)
					{
						m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_ROOT_NODE, m_strFileSpec));
						return false;
					}
					
					return true;
				}
			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_FILE_EX, m_strFileSpec), Ex);
			}
			
			return false;

		}//	public virtual bool Open(bool bCreate)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method is called to create the XML file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file being created</param>
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
					
					//	Make sure the comments have been populated
					SetComments();
					
					//	Write all comments to file
					if((m_aComments != null) && (m_aComments.Count > 0))
					{
						foreach(object objComment in m_aComments)
						{
							xmlWriter.WriteComment(objComment.ToString());
						}
					}
					
					xmlWriter.WriteStartElement(m_strRoot);
					xmlWriter.WriteEndElement();
					xmlWriter.WriteEndDocument();
					xmlWriter.Close();
					
					return true;

				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Create", m_tmaxErrorBuilder.Message(ERROR_CREATE_FILE_EX, strFileSpec), Ex);
			}
			
			return false;
		
		}//	protected virtual bool Create(string strFileSpec)
		
		/// <summary>This method is called to populate the comments array prior to creating the file</summary>
		protected virtual void SetComments()
		{
		
		}// protected virtual void SetComments()
		
		/// <summary>This method will populate the local error builder's format string collection</summary>
		protected override void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	Do the base class processing first
			base.SetErrorStrings();
			
			//	The format strings must be added in the order in which they are defined
			aStrings.Add("An exception was raised while attempting to create the XML file: filename = %1");
			aStrings.Add("An exception was raised while attempting to open the XML file: filename = %1");
			aStrings.Add("An exception was raised while attempting to save the XML file: filename = %1");
			aStrings.Add("An exception was raised while attempting to write an object to the XML file: filename = %1");
			aStrings.Add("Unable to locate the XML file: filename = %1");
			aStrings.Add("%1 is not a valid XML file. It does not contain a root node.");
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		/// <summary>Controls whether or not the current date is added to the specified filename</summary>
		public bool AddDateToFilename
		{
			get { return m_bAddDateToFilename; }
			set { m_bAddDateToFilename = value; }
		}

		/// <summary>Folder (path) used to contruct the full path specification of the log file</summary>
		public string Folder
		{
			get { return m_strFolder; }
			set { m_strFolder = value; }
		}

		/// <summary>Name of root element in the file</summary>
		public string Root
		{
			get { return m_strRoot; }
			set { m_strRoot = value; }
		}

		/// <summary>Collection of comments added to file header when created</summary>
		public ArrayList Comments
		{
			get { return m_aComments; }
		}

		/// <summary>Xml document object associated with the file</summary>
		public XmlDocument Document
		{
			get { return m_xmlDocument; }
		}

		/// <summary>Filename used to contruct the full path specification of the log file</summary>
		public string Filename
		{
			get { return m_strFilename; }
			set { m_strFilename = value; }
		}

		/// <summary>Extension used to contruct the full path specification of the log file</summary>
		public string Extension
		{
			get { return m_strExtension; }
			set { m_strExtension = value; }
		}

		/// <summary>Full file specification for the log file</summary>
		public string FileSpec
		{
			get { return GetFileSpec(); }
			set { SetFileProps(value); }
		}

		#endregion Properties
		
	}//	public class CXmlFile : CXmlBase
	
}// namespace FTI.Shared.Xml
