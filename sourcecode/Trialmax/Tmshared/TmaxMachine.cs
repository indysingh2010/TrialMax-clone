using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class manages the information associated with a machine connected to a case</summary>
	public class CTmaxMachine
	{
		#region Constants
		
		private const string XMLINI_PATH_MAP			= "PathMap";
		private const string XMLINI_USER				= "User";
		private const string XMLINI_OPENED				= "Opened";
		private const string XMLINI_CLOSED				= "Closed";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to PathMap property</summary>
		private int	m_iPathMap = -1;
		
		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "";
		
		/// <summary>Local member bound to User property</summary>
		private string m_strUser = "";
		
		/// <summary>Local member bound to Opened property</summary>
		private string m_strOpened = "";
		
		/// <summary>Local member bound to Closed property</summary>
		private string m_strClosed = "";
		
		#endregion Private Members
	
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CTmaxMachine()
		{
			Initialize("");
		}
		
		/// <summary>Constructor</summary>
		public CTmaxMachine(string strName)
		{
			Initialize(strName);
		}
		
		/// <summary>This method is called to get the name of the section in the XML configuration file for this machine</summary>
		/// <returns>The name of the section containing the configuration information for this machine</returns>
		public string GetXmlSectionName()
		{
			if(m_strName.Length > 0)
				return (GetXmlSectionPrefix() + m_strName);
			else
				return "";
		
		}// public string GetXmlSectionName()
		
		/// <summary>This method is called to get the prefix used to construct the XML section name for this machine</summary>
		/// <returns>The prefix of the section containing the configuration information for this machine</returns>
		static public string GetXmlSectionPrefix()
		{
			return ("trialMax/machine/");
		}
		
		/// <summary>This method is called to load the options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the application option values</param>
		public void Load(CXmlIni xmlIni)
		{
			string strOldSection = "";
			
			if(GetXmlSectionName().Length == 0) return;
			
			//	Line up on the section that has the same name
			strOldSection = xmlIni.Section;
			
			if(xmlIni.SetSection(GetXmlSectionName()) == true)
			{
				m_iPathMap = xmlIni.ReadInteger(XMLINI_PATH_MAP, m_iPathMap);
				m_strUser = xmlIni.Read(XMLINI_USER, m_strUser);
				m_strOpened = xmlIni.Read(XMLINI_OPENED, m_strOpened);
				m_strClosed = xmlIni.Read(XMLINI_CLOSED, m_strClosed);
			
			}// if(xmlIni.SetSection(GetXmlSectionName()) == true)
			
			//	Restore the section
			if(strOldSection.Length > 0)
				xmlIni.SetSection(strOldSection);
				
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the option values</param>
		public void Save(CXmlIni xmlIni)
		{
			string strOldSection = "";
			
			if(GetXmlSectionName().Length == 0) return;
			
			//	Line up on the section that has the same name
			strOldSection = xmlIni.Section;
			
			if(xmlIni.SetSection(GetXmlSectionName()) == true)
			{
				xmlIni.Write(XMLINI_PATH_MAP, m_iPathMap);
				xmlIni.Write(XMLINI_USER, m_strUser);
				xmlIni.Write(XMLINI_OPENED, m_strOpened);
				xmlIni.Write(XMLINI_CLOSED, m_strClosed);

			}// if(xmlIni.SetSection(GetXmlSectionName()) == true)
			
			//	Restore the section
			if(strOldSection.Length > 0)
				xmlIni.SetSection(strOldSection);

		}// public void Save(CXmlIni xmlIni)
		
		/// <summary>This method initializes the properties to their default values</summary>
		/// <param name="strName">The name to be assigned to the machine</param>
		public void Initialize(string strName)
		{
			m_strName = "";
			m_strUser = "";
			m_iPathMap = -1;
			m_strOpened = System.DateTime.Now.ToString();
			m_strClosed = "";
		
			if((strName != null) && (strName.Length > 0))
			{
				m_strName = strName;
			}
			else if(Environment.MachineName.Length > 0)
			{
				m_strName = Environment.MachineName;
			}
			
		}// public void Initialize(string strName)
			
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Identifier of path map used to access the case's media files</summary>
		public int PathMap
		{
			get { return m_iPathMap; }
			set { m_iPathMap = value; }
		}
		
		/// <summary>Network name of the machine</summary>
		public string Name
		{
			get 
			{ 
				if(m_strName.Length > 0)
					return m_strName;
				else if(Environment.MachineName.Length > 0)
					return Environment.MachineName;
				else
					return "Stand-alone";
			}
			
			set { m_strName = value; }
		
		}// Name property
		
		/// <summary>Time when the last user opened the case</summary>
		public string Opened
		{
			get { return m_strOpened; }
			set { m_strOpened = value; }
		}
		
		/// <summary>Time when the last user closed the case</summary>
		public string Closed
		{
			get { return m_strClosed; }
			set { m_strClosed = value; }
		}
		
		/// <summary>Name of the last known user</summary>
		public string User
		{
			get 
			{ 
				if(m_strUser.Length > 0)
					return m_strUser;
				else
					return "Unknown User";
			}
			
			set { m_strUser = value; }
		
		}
		
		#endregion Properties
	
	}// public class CTmaxMachine

}// namespace FTI.Shared.Trialmax
