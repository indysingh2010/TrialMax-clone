using System.Xml;
using System.Xml.XPath;
using System.Diagnostics;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;

namespace FTI.Shared.Xml
{
	/// <summary>This class manages the information associated with an XML deposition node</summary>
	public class CXmlAdobeConversion : CXmlFile
	{
		#region Constants
		
		public const string XML_ROOT_NAME = "FTIP2I";

		protected const int ERROR_OPEN_EX				= (LAST_XML_FILE_ERROR + 1);
		protected const int	ERROR_INVALID_PAGE_COUNT	= (LAST_XML_FILE_ERROR + 2);
		protected const int	ERROR_SET_PROPERTIES_EX		= (LAST_XML_FILE_ERROR + 3);
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Local member bound to Pages property</summary>
		private int m_iPages = 0;

		/// <summary>Local member bound to Program property</summary>
		private string m_strProgram = "";

		/// <summary>This member is bounded to the Version property</summary>
		protected string m_strVersion = "";		

		/// <summary>Local member bound to TimeStarted property</summary>
		private string m_strTimeStarted = "";

		/// <summary>Local member bound to TimeCompleted property</summary>
		private string m_strTimeCompleted = "";

		/// <summary>Local member bound to Warning property</summary>
		private string m_strWarning = "";

		/// <summary>Local member bound to Error property</summary>
		private string m_strError = "";

		/// <summary>Local member bound to FatalError property</summary>
		private string m_strFatalError = "";

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CXmlAdobeConversion()
		{
			m_strRoot = XML_ROOT_NAME;
			Extension = GetExtension();
			
			//	Assign the default filename
			Filename = "conv.xml";
			
			//	Populate the error builder
			SetErrorStrings();
		}
		
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
					//	Use the XML root node to set the properties for this object
					if(SetProperties(m_xmlRoot) == false)
						break;
						
					//	We're done 
					bSuccessful = true;
					
				}//	while(bSuccessful == false)
		
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "Open", m_tmaxErrorBuilder.Message(ERROR_OPEN_EX, m_strFileSpec), Ex);
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
		
		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <returns>true if successful</returns>
		public override bool Open()
		{	
			//	We don't create Adobe conversion result files
			return Open(false);
		}
			
		/// <summary>This method is called to get open the file and load the Xml document object</summary>
		/// <param name="strFileSpec">Fully qualified path to the file</param>
		/// <returns>true if successful</returns>
		public override bool Open(string strFileSpec)
		{
			//	We don't create Adobe conversion result files
			return Open(strFileSpec, false);
		}
			
		/// <summary>This method is called to get the string representation of this file</summary>
		/// <returns>The text that represents this object</returns>
		public override string ToString()
		{
			string strString = "";
			
			strString  = ("FileSpec: " + this.FileSpec.ToLower() + "\n");
			strString += ("Pages: " + this.Pages.ToString() + "\n");
			strString += ("Program: " + this.Program + "\n");
			strString += ("Version: " + this.Version + "\n");
			strString += ("TimeStarted: " + this.TimeStarted + "\n");
			strString += ("TimeCompleted: " + this.TimeCompleted + "\n");
			strString += ("Warning: " + this.Warning + "\n");
			strString += ("Error: " + this.Error + "\n");
			strString += ("FatalError: " + this.FatalError + "\n");
			
			return strString;
			
		}// public override string ToString()

		#endregion Public Methods
		
		#region Protected Methods
		
		/// <summary>This method uses the specified node to set the deposition properties</summary>
		/// <param name="xmlNode">The node used to initialize the properties</param>
		/// <returns>true if successful</returns>
		protected bool SetProperties(XmlNode xmlNode)
		{
			XmlNode xmlElement = null;
			bool	bSuccessful = false;
			
			Debug.Assert(xmlNode != null);
			
			try
			{
				while(bSuccessful == false)
				{
					//	Get the page count
					if((xmlElement = xmlNode.SelectSingleNode("pages")) != null)
					{
						try
						{
							m_iPages = System.Convert.ToInt32(xmlElement.InnerText);
						}
						catch
						{
						}
					}
					
					//	Fire an error if we did not get a valid page count
					if(m_iPages < 0)
					{
						m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_INVALID_PAGE_COUNT, m_strFileSpec));
					}
					
					//	Get the program name
					if((xmlElement = xmlNode.SelectSingleNode("program")) != null)
						m_strProgram = xmlElement.InnerText;
						
					//	Get the version
					if((xmlElement = xmlNode.SelectSingleNode("version")) != null)
						m_strVersion = xmlElement.InnerText;
						
					//	Get the start time
					if((xmlElement = xmlNode.SelectSingleNode("timestarted")) != null)
						m_strTimeStarted = xmlElement.InnerText;
						
					//	Get the completion time
					if((xmlElement = xmlNode.SelectSingleNode("timecompleted")) != null)
						m_strTimeCompleted = xmlElement.InnerText;
						
					//	Get the conversion warning
					if((xmlElement = xmlNode.SelectSingleNode("warning")) != null)
						m_strWarning = xmlElement.InnerText;
						
					//	Get the conversion error
					if((xmlElement = xmlNode.SelectSingleNode("error")) != null)
						m_strError = xmlElement.InnerText;
						
					//	Get the conversion fatal error
					if((xmlElement = xmlNode.SelectSingleNode("fatalerror")) != null)
						m_strFatalError = xmlElement.InnerText;
						
					//	Everything is OK
					bSuccessful = true;
					
				}// protected bool SetProperties(XmlNode xmlNode)
				
				//	Raise an error if the file is not valid
				if(bSuccessful == false)
				{
				}

			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireError(this, "SetProperties", m_tmaxErrorBuilder.Message(ERROR_SET_PROPERTIES_EX, m_strFileSpec), Ex);
			}

			return bSuccessful;
			
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
			aStrings.Add("An exception was raised while attempting to open the PDF conversion results: filename = %1");
			aStrings.Add("Unable to retrieve a valid page count from the PDF conversion results file: filename = %1");
			aStrings.Add("An exception was raised while attempting to read the PDF conversion results: filename = %1");
		}
		
		#endregion Protected Methods
		
		#region Properties
		
		//	Conversion page count
		public int Pages
		{
			get{ return m_iPages; }
		}
		
		//	Converter application program name
		public string Program
		{
			get{ return m_strProgram; }
		}
		
		//	Converter application version identifer
		public string Version
		{
			get{ return m_strVersion; }
		}
		
		//	Conversion start time
		public string TimeStarted
		{
			get{ return m_strTimeStarted; }
		}
		
		//	Conversion completion time
		public string TimeCompleted
		{
			get{ return m_strTimeCompleted; }
		}
		
		//	Conversion warning
		public string Warning
		{
			get{ return m_strWarning; }
		}
		
		//	Conversion error
		public string Error
		{
			get{ return m_strError; }
		}
		
		//	Conversion fatal error
		public string FatalError
		{
			get{ return m_strFatalError; }
		}
		
		#endregion Properties
		
	}// public class CXmlAdobeConversion

}// namespace FTI.Shared.Xml
