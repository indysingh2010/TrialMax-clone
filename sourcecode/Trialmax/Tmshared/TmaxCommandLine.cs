using System;
using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the command line for an instance of the application</summary>
	public class CTmaxCommandLine : CXmlFile
	{
		#region Constants
		
		public const string XML_COMMAND_LINE_ROOT_NAME = "trialMax";
		public const string XML_COMMAND_LINE_ELEMENT_NAME = "commandLine";

		public const string XML_COMMAND_LINE_ATTRIBUTE_CASE_FOLDER = "caseFolder";
		public const string XML_COMMAND_LINE_ATTRIBUTE_SOURCE_FILE = "sourceFile";
		public const string XML_COMMAND_LINE_ATTRIBUTE_VIDEO_PATH = "videoPath";
		public const string XML_COMMAND_LINE_ATTRIBUTE_APP_ID = "appId";

		protected const int ERROR_OPEN_COMMAND_LINE_EX	= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_NO_COMMAND_LINE_NODE	= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX		= (LAST_XML_FILE_ERROR + 3);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>This member is bound to the AppId property</summary>
		private TmaxApplications m_eAppId = TmaxApplications.TmaxManager;		
		
		/// <summary>This member is bound to the CaseFolder property</summary>
		private string m_strCaseFolder = "";		
		
		/// <summary>This member is bound to the SourceFile property</summary>
		private string m_strSourceFile = "";		
		
		/// <summary>This member is bound to the VideoPath property</summary>
		private string m_strVideoPath = "";		
		
		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxCommandLine() : base()
		{
			Initialize(TmaxApplications.TmaxManager);
		}
		
		/// <summary>Constructor</summary>
		/// <param name="eAppId">The TrialMax application identifier</param>
		public CTmaxCommandLine(TmaxApplications eAppId) : base()
		{
			Initialize(eAppId);
		}
		
		/// <summary>This method is called to open the specified file</summary>
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
					//	Get the command line descriptor node
					if((xmlNode = m_xmlRoot.SelectSingleNode(XML_COMMAND_LINE_ELEMENT_NAME)) == null)
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
							m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_NO_COMMAND_LINE_NODE, m_strFileSpec));
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
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_COMMAND_LINE_EX, m_strFileSpec), Ex);
			}
			
			if(bSuccessful == false)
			{
				Close();
				return false;
			}
			else
			{
				return true;
			}
			
		}//	Open(bool bCreate)
		
		/// <summary>This method will set the designation properties using the specified node</summary>
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
		
		/// <summary>This method will set the designation properties using the specified navigator</summary>
		/// <param name="xpNavigator">The navigator used to iterate the attributes</param>
		/// <returns>true if successful</returns>
		public bool SetProperties(XPathNavigator xpNavigator)
		{
			string strAttribute = "";
			
			Debug.Assert(xpNavigator != null);
			
			try
			{
				strAttribute = xpNavigator.GetAttribute(XML_COMMAND_LINE_ATTRIBUTE_APP_ID,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_eAppId = (TmaxApplications)(System.Convert.ToInt32(strAttribute));

				strAttribute = xpNavigator.GetAttribute(XML_COMMAND_LINE_ATTRIBUTE_CASE_FOLDER,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strCaseFolder = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_COMMAND_LINE_ATTRIBUTE_SOURCE_FILE,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strSourceFile = strAttribute;

				strAttribute = xpNavigator.GetAttribute(XML_COMMAND_LINE_ATTRIBUTE_VIDEO_PATH,"");
				if((strAttribute != null) && (strAttribute.Length > 0))
					m_strVideoPath = strAttribute;

				return true;
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX), Ex);
				return false;
			}
			
		}// public bool SetProperties(XPathNavigator xpNavigator)
		
		/// <summary>This method set the properties using the specified command line arguments</summary>
		/// <param name="args">The array of command line arguments</param>
		public void SetProperties(string[] args)
		{
			if((args != null) && (args.GetUpperBound(0) >= 0))
			{
				if(m_eAppId == TmaxApplications.VideoViewer)
				{
					for(int i = 0; i <= args.GetUpperBound(0); i++)
					{
						if(String.Compare(args[i], 0, "Source", 0, 6, true) == 0)
						{
							m_strSourceFile = GetValue(args[i]);
						}
						else if(String.Compare(args[i], 0, "Video", 0, 5, true) == 0)
						{
							m_strVideoPath = GetValue(args[i]);
						}
						
					}// for(int i = 0; i <= args.GetUpperBound(0); i++)
					
				}
				else
				{
					//	Manager only supports one command line parameter
					m_strCaseFolder = args[0];
				}
				
			}// if((args != null) && (args.GetUpperBound(0) >= 0))

		}// public void SetProperties(string[] args)
		
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
				System.IO.File.Delete(m_strFileSpec);
			}
			
			try
			{

				//	Open the file
				if(Open(true) == true)
				{
					Debug.Assert(m_xmlDocument != null);
					Debug.Assert(m_xmlRoot != null);
					
					//	Get the node that represents the command line
					if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					{
						m_xmlRoot.AppendChild(xmlChild);
						
						if((xmlWriter = new XmlTextWriter(m_strFileSpec, null)) != null)
						{
							xmlWriter.Formatting = System.Xml.Formatting.Indented;
							xmlWriter.Indentation = 4;
							
							m_xmlDocument.Save(xmlWriter);
							xmlWriter.Close();
							
							//	Close the document
							Close();

							return true;
						}
						
					}// if((xmlChild = ToXmlNode(m_xmlDocument)) != null)
					
				}
				
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Save", m_tmaxErrorBuilder.Message(ERROR_SAVE_FILE_EX, m_strFileSpec), Ex);
			}
			
			return false;
		
		}//	Save()
		
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
				strElementName = XML_COMMAND_LINE_ELEMENT_NAME;
				
			if((xmlElement = xmlDocument.CreateElement(strElementName)) != null)
			{
				while(bSuccessful == false)
				{
					if(AddAttribute(xmlElement, XML_COMMAND_LINE_ATTRIBUTE_APP_ID, ((int)m_eAppId).ToString()) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_COMMAND_LINE_ATTRIBUTE_CASE_FOLDER, m_strCaseFolder) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_COMMAND_LINE_ATTRIBUTE_SOURCE_FILE, m_strSourceFile) == false)
						break;
						
					if(AddAttribute(xmlElement, XML_COMMAND_LINE_ATTRIBUTE_VIDEO_PATH, m_strVideoPath) == false)
						break;
						
					//	We're done
					bSuccessful = true;
				
				}// while(1)
				
			}
			
			return (bSuccessful == true) ? xmlElement : null;

		}// public override XmlNode ToXmlNode(XmlDocument xmlDocument, string strName)
		
		/// <summary>Overloaded base class member to provide a text representation of the object</summary>
		/// <returns>The text representation</returns>
		public override string ToString()
		{
			string strCmdLine = "";
			
			strCmdLine += ("AppId = " + m_eAppId.ToString() + "\n");
			strCmdLine += ("CaseFolder = " + m_strCaseFolder + "\n");
			strCmdLine += ("SourceFile = " + m_strSourceFile + "\n");
			strCmdLine += ("VideoPath = " + m_strVideoPath + "\n");
			
			return strCmdLine;
		}
		
		/// <summary>Called to initialize the class members</summary>
		/// <param name="eAppId">The TrialMax application identifier</param>
		public void Initialize(TmaxApplications eAppId)
		{
			m_eAppId = eAppId;
			m_strRoot = XML_COMMAND_LINE_ROOT_NAME;
			m_strFilename = "_tmax_command_line";
			m_strExtension = ".xml";

		}// public void Initialize(TmaxApplications eAppId)
		
		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method extracts the value from the specified command line option</summary>
		/// <param name="args">The array of command line arguments</param>
		protected string GetValue(string strOption)
		{
			string	strValue = "";
			int		iIndex = 0;
			
			if((iIndex = strOption.IndexOf('=')) >= 0)
			{
				strValue = strOption.Substring(iIndex + 1);
				strValue = strValue.Trim();
			}
			
			return strValue;
				
		}// protected string GetValue(string strOption)
		
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
			aStrings.Add("An exception was raised while attempting to open the command line file: %1");
			aStrings.Add("%1 is not a valid XML command line file. It does not contain a commandLine node.");
			aStrings.Add("An exception was raised while attempting to set the commandLine properties");
			
		}// protected override void SetErrorStrings()
		
		#endregion Protected Methods
		
		#region Properties
		
		//	The application identifier
		public TmaxApplications AppId
		{
			get{ return m_eAppId; }
			set{ m_eAppId = value; }
		}
		
		//	Folder containing the case to be opened
		public string CaseFolder
		{
			get{ return m_strCaseFolder; }
			set{ m_strCaseFolder = value; }
		}
		
		//	Source file specified on the command line
		public string SourceFile
		{
			get{ return m_strSourceFile; }
			set{ m_strSourceFile = value; }
		}
		
		//	Path to video files
		public string VideoPath
		{
			get{ return m_strVideoPath; }
			set{ m_strVideoPath = value; }
		}
		
		#endregion Properties
		
	}// public class CTmaxCommandLine

}// namespace FTI.Shared.Trialmax
