using System;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;

using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the case options specific to an individual work station</summary>
	public class CTmaxStationOptions
	{
		#region Constants
		
		private const string XMLINI_USER_KEY				= "User";
		private const string XMLINI_OPENED_KEY				= "Opened";
		private const string XMLINI_CLOSED_KEY				= "Closed";
		private const string XMLINI_LAST_HIGHLIGHTER_KEY	= "LastHighlighter";
		private const string XMLINI_LAST_DEPOSITION_KEY		= "LastDeposition";
		private const string XMLINI_TARGET_BINDER_KEY		= "TargetBinderId";
		private const string XMLINI_PAUSE_THRESHOLD_KEY		= "PauseThreshold";
		private const string XMLINI_ADD_OBJECTIONS_KEY		= "AddObjections";

		private const string ADD_OBJECTIONS_ATTRIBUTE_DEPOSITION	= "deposition";
		private const string ADD_OBJECTIONS_ATTRIBUTE_PLAINTIFF		= "plaintiff";
		private const string ADD_OBJECTIONS_ATTRIBUTE_ARGUMENT		= "argument";

		private const int DEFAULT_PAUSE_THRESHOLD = 12;
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();
		
		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();		
		
		/// <summary>Local member used to manage the configuration file</summary>
		private FTI.Shared.Xml.CXmlIni m_xmlIni = null;
		
		/// <summary>Local member bound to CaseId property</summary>
		private string m_strCaseId = "";
		
		/// <summary>Local member bound to User property</summary>
		private string m_strUser = "";
		
		/// <summary>Local member bound to Opened property</summary>
		private string m_strOpened = "";
		
		/// <summary>Local member bound to Closed property</summary>
		private string m_strClosed = "";
		
		/// <summary>Local member bound to LastHighlighter property</summary>
		private long m_lLastHighlighter = -1;
		
		/// <summary>Local member bound to LastDeposition property</summary>
		private long m_lLastDeposition = -1;
		
		/// <summary>Local member bound to TargetBinderId property</summary>
		private long m_lTargetBinderId = -1;
		
		/// <summary>Local member bound to Import property</summary>
		private CTmaxImportOptions m_tmaxImportOptions = new CTmaxImportOptions();

		/// <summary>Local member bound to ExportOptions property</summary>
		private CTmaxExportOptions m_tmaxExportOptions = new CTmaxExportOptions();

		/// <summary>Local member bound to ExportObjections property</summary>
		private CTmaxExportObjections m_tmaxExportObjections = new CTmaxExportObjections();

		/// <summary>Local member bound to TrimOptions property</summary>
		private CTmaxTrimOptions m_tmaxTrimOptions = new CTmaxTrimOptions();

		/// <summary>Local member bound to AdvancedFilter property</summary>
		private CTmaxFilter m_tmaxAdvancedFilter = new CTmaxFilter();

		/// <summary>Local member bound to PauseThreshold property</summary>
		private int m_iPauseThreshold = DEFAULT_PAUSE_THRESHOLD;

		/// <summary>Local member bound to AddObjectionDeposition property</summary>
		private string m_strAddObjectionDeposition = "";

		/// <summary>Local member bound to AddObjectionArgument property</summary>
		private string m_strAddObjectionArgument = "";

		/// <summary>Local member bound to AddObjectionPlaintiff property</summary>
		private bool m_bAddObjectionPlaintiff = false;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Default constructor</summary>
		public CTmaxStationOptions()
		{
			//	Initialize the event handling
			SetErrorStrings();
			m_tmaxEventSource.Name = "Station Options Events";
			m_tmaxEventSource.Attach(m_tmaxExportOptions.EventSource);
			m_tmaxEventSource.Attach(m_tmaxImportOptions.EventSource); 
			m_tmaxEventSource.Attach(m_tmaxTrimOptions.EventSource);
			m_tmaxEventSource.Attach(m_tmaxAdvancedFilter.EventSource);
			m_tmaxEventSource.Attach(m_tmaxExportObjections.EventSource); 
			
		}// CTmaxStationOptions()
		
		/// <summary>Called to initialize the object for operation with a new case</summary>
		/// <param name="strFileSpec">The path to the configuration file containing the values</param>
		/// <param name="strCaseId">The unique id of the case</param>
		/// <param name="strUser">The name of the active user</param>
		/// <returns>true if succesful</returns>
		public bool Initialize(string strFileSpec, string strCaseId, string strUser)
		{
			//	Case Id and user id are not stored in the XML file
			SetCaseId(strCaseId);
			m_strUser = strUser != null ? strUser : "";

			//	Reset the class members
			m_strOpened = "";
			m_strClosed = "";
			m_lLastHighlighter = -1;
			m_lLastDeposition = -1;
			m_lTargetBinderId = -1;
			m_iPauseThreshold = DEFAULT_PAUSE_THRESHOLD;
			m_bAddObjectionPlaintiff = false;
			m_strAddObjectionDeposition = "";
			m_strAddObjectionArgument = "";
			m_tmaxImportOptions.Clear();
			m_tmaxExportOptions.Clear();
			m_tmaxTrimOptions.Clear();
			m_tmaxAdvancedFilter.Clear();
			m_tmaxExportObjections.Clear();
			
			//	The nested objects use the CaseId as their unique Name
			m_tmaxImportOptions.Name = m_strCaseId;
			m_tmaxExportOptions.Name = m_strCaseId;
			m_tmaxTrimOptions.CaseId = m_strCaseId;
			m_tmaxAdvancedFilter.Name = m_strCaseId;
			m_tmaxExportObjections.Name = m_strCaseId;

			return SetFileSpec(strFileSpec, true);
			
		}// public void Initialize(string strFileSpec, string strCaseId, string strUser)
		
		/// <summary>This method is called to save the values to the XML configuration file</summary>
		/// <returns>True if successful</returns>
		public bool Save()
		{
			bool bSuccessful = false;
			
			while(bSuccessful == false)
			{
				//	Make sure the file is opened
				if(m_xmlIni == null)
					break;

				//	Write the values to the file
				if(Save(m_xmlIni) == false) 
					break;
			
				//	Save the configuration file
				if(m_xmlIni.Save() == false)
					break;
					
				bSuccessful = true;
				
			}// while(bSuccessful == false)
			
			return bSuccessful;
			
		}// public bool Save()
		
		/// <summary>This method is called to clear the collections and reset the class members</summary>
		public void Clear()
		{
			Initialize("", "", "");		
		}

		/// <summary>This method is called to set the target binder id</summary>
		public void SetTargetBinderId(long lId)
		{
			m_lTargetBinderId = lId;
			
		}// public void SetTargetBinderId(long lId)

		#endregion Public Methods
		
		#region Private Methods
		
		/// <summary>This method closes the XML file</summary>
		private void Close()
		{
			//	Has the XML file been opened?
			if(m_xmlIni != null)
			{
				m_xmlIni.Close();
				m_xmlIni = null;
			}
			
		}// private void Close()
		
		/// <summary>This method is called to open the specified XML configuration file</summary>
		/// <param name="strFileSpec">The fully qualified path to the file</param>
		/// <param name="bLoad">true to load the values stored in the file</param>
		/// <returns>true if successful</returns>
		private bool Open(string strFileSpec, bool bLoad)
		{
			bool bSuccessful = false;
			
			try
			{
				//	Make sure the existing file is closed
				Close();
			
				Debug.Assert(strFileSpec.Length > 0);

				//	Allocate and initialize a new XML file object
				m_xmlIni = new CXmlIni();
				
				m_xmlIni.XMLComments.Add("TrialMax .NET XML Station Configuration File");
				m_xmlIni.XMLComments.Add("Copyright FTI Consulting");
				
				//	Load the values stored in the configuration file
				while(bSuccessful == false)
				{
					//	Open the file
					if(m_xmlIni.Open(strFileSpec) == false)
						break;
						
					//	Read the options from the file
					if(bLoad == true)
					{
						if(Load(m_xmlIni) == false)
							break;
					}
						
					bSuccessful = true;
				
				}// while(bSuccessful == false)
			
			}
			catch
			{
				//	Make sure the file is closed
				Close();
			}
					
			return bSuccessful;
		
		}// private bool Open(string strFileSpec)
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the values are stored</param>
		/// <returns>true if successful</returns>
		private bool Load(CXmlIni xmlIni)
		{
			string	strSection = "";
			
			//	Make sure we have a valid section name
			strSection = GetXmlSectionName();
			if(strSection.Length == 0) return false;
			
			if(xmlIni.SetSection(strSection) == false) return false;
			
			m_strUser = xmlIni.Read(XMLINI_USER_KEY, m_strUser);
			m_strOpened = xmlIni.Read(XMLINI_OPENED_KEY, m_strOpened);
			m_strClosed = xmlIni.Read(XMLINI_CLOSED_KEY, m_strClosed);
			m_lLastHighlighter = xmlIni.ReadLong(XMLINI_LAST_HIGHLIGHTER_KEY, m_lLastHighlighter);
			m_lLastDeposition = xmlIni.ReadLong(XMLINI_LAST_DEPOSITION_KEY, m_lLastDeposition);
			m_lTargetBinderId = xmlIni.ReadLong(XMLINI_TARGET_BINDER_KEY, m_lTargetBinderId);
			m_iPauseThreshold = xmlIni.ReadInteger(XMLINI_PAUSE_THRESHOLD_KEY, m_iPauseThreshold);
			
			m_bAddObjectionPlaintiff = xmlIni.ReadBool(XMLINI_ADD_OBJECTIONS_KEY, ADD_OBJECTIONS_ATTRIBUTE_PLAINTIFF, false);
			m_strAddObjectionDeposition = xmlIni.Read(XMLINI_ADD_OBJECTIONS_KEY, ADD_OBJECTIONS_ATTRIBUTE_DEPOSITION, "");
			m_strAddObjectionArgument = xmlIni.Read(XMLINI_ADD_OBJECTIONS_KEY, ADD_OBJECTIONS_ATTRIBUTE_ARGUMENT, "");

			//	Load the nested options
			m_tmaxImportOptions.Load(xmlIni);
			m_tmaxExportOptions.Load(xmlIni);
			m_tmaxTrimOptions.Load(xmlIni);
			m_tmaxAdvancedFilter.Load(xmlIni);
			m_tmaxExportObjections.Load(xmlIni);
			
			return true;
			
		}// private bool Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the application option values</param>
		/// <returns>true if successful</returns>
		private bool Save(CXmlIni xmlIni)
		{
			string strSection = "";
			
			//	Make sure we have a valid section name
			strSection = GetXmlSectionName();
			if(strSection.Length == 0) return false;

			if(xmlIni.SetSection(strSection, true) == false) return false;
			
			xmlIni.Write(XMLINI_USER_KEY, m_strUser);
			xmlIni.Write(XMLINI_OPENED_KEY, m_strOpened);
			xmlIni.Write(XMLINI_CLOSED_KEY, m_strClosed);
			xmlIni.Write(XMLINI_LAST_HIGHLIGHTER_KEY, m_lLastHighlighter);
			xmlIni.Write(XMLINI_LAST_DEPOSITION_KEY, m_lLastDeposition);
			xmlIni.Write(XMLINI_TARGET_BINDER_KEY, m_lTargetBinderId);
			xmlIni.Write(XMLINI_PAUSE_THRESHOLD_KEY, m_iPauseThreshold);

			xmlIni.Write(XMLINI_ADD_OBJECTIONS_KEY, ADD_OBJECTIONS_ATTRIBUTE_PLAINTIFF, m_bAddObjectionPlaintiff);
			xmlIni.Write(XMLINI_ADD_OBJECTIONS_KEY, ADD_OBJECTIONS_ATTRIBUTE_DEPOSITION, m_strAddObjectionDeposition);
			xmlIni.Write(XMLINI_ADD_OBJECTIONS_KEY, ADD_OBJECTIONS_ATTRIBUTE_ARGUMENT, m_strAddObjectionArgument);

			//	Save the nested options
			m_tmaxImportOptions.Save(xmlIni);
			m_tmaxExportOptions.Save(xmlIni);
			m_tmaxTrimOptions.Save(xmlIni);
			m_tmaxAdvancedFilter.Save(xmlIni);
			m_tmaxExportObjections.Save(xmlIni);

			return true;
			
		}// private bool Save(CXmlIni xmlIni)
		
		/// <summary>This method is called to save the save the case code attributes</summary>
		/// <param name="tmaxCaseCodes">The application's collection of case codes</param>
		/// <returns>True if successful</returns>
		public bool SaveCaseCodes(CTmaxCaseCodes tmaxCaseCodes)
		{
			string	strSection = "";
			string	strKey = "";
			bool	bSuccessful = false;
			
			if(tmaxCaseCodes == null) return false;
			if(tmaxCaseCodes.Count == 0) return true; // Nothing to save
			
			//	The file MUST be open
			if(m_xmlIni == null) return false;
			
			try
			{
				//	Line up on the appropriate section
				strSection = ("trialMax/station/caseCodes/" + m_strCaseId);
				if(m_xmlIni.SetSection(strSection, true, true) == false) return false;
				
				//	Write the case codes to file
				foreach(CTmaxCaseCode O in tmaxCaseCodes)
				{
					strKey = String.Format("CC{0}", O.UniqueId);
					m_xmlIni.Write(strKey, O.Hidden);
				}
				
				bSuccessful = true;			
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "SaveCaseCodes", Ex);
			}
			
			return bSuccessful;
			
		}// public void SaveCaseCodes(CTmaxCaseCodes tmaxCaseCodes)
		
		/// <summary>This method is called to load the case code attributes</summary>
		/// <param name="tmaxCaseCodes">The application's collection of case codes</param>
		/// <returns>True if successful</returns>
		public bool LoadCaseCodes(CTmaxCaseCodes tmaxCaseCodes)
		{
			string	strSection = "";
			bool	bSuccessful = false;
			string	strKey = "";
			
			if(tmaxCaseCodes == null) return false;
			if(tmaxCaseCodes.Count == 0) return true; // Nothing to load
			
			try
			{
				//	The file MUST be open
				if(m_xmlIni != null)
				{			
					//	Line up on the appropriate section
					strSection = ("trialMax/station/caseCodes/" + m_strCaseId);
					if(m_xmlIni.SetSection(strSection, false, false) == false) return false;
					
					//	Load the case codes
					foreach(CTmaxCaseCode O in tmaxCaseCodes)
					{
						strKey = String.Format("CC{0}", O.UniqueId);
						O.Hidden = m_xmlIni.ReadBool(strKey, true);
					}
					
					bSuccessful = true;	
				}
				else
				{
					//	Turn all codes on by default
					foreach(CTmaxCaseCode O in tmaxCaseCodes)
						O.Hidden = true;
				}
						
			}
			catch(System.Exception Ex)
			{
				m_tmaxEventSource.FireDiagnostic(this, "LoadCaseCodes", Ex);
			}
			
			return bSuccessful;
			
		}// public bool LoadCaseCodes(CTmaxCaseCodes tmaxCaseCodes)
		
		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		private string GetXmlSectionName()
		{
			if(m_strCaseId.Length > 0)
				return ("trialMax/station/options/" + m_strCaseId);
			else
				return "";
		
		}// private string GetXmlSectionName()
		
		/// <summary>This method is called to set the unique case id</summary>
		/// <param name="strCaseId">The case id stored in the database</param>
		private void SetCaseId(string strCaseId)
		{
			if(strCaseId != null)
				m_strCaseId = strCaseId;
			else
				m_strCaseId = "";
				
			//	Make sure the name of the nested objects match
			m_tmaxImportOptions.Name = m_strCaseId;
			m_tmaxExportOptions.Name = m_strCaseId;
			m_tmaxTrimOptions.CaseId = m_strCaseId;
			m_tmaxAdvancedFilter.Name = m_strCaseId;
			m_tmaxExportObjections.Name = m_strCaseId;
			
		}// private void SetCaseId(string strCaseId)
			
		/// <summary>This method is called to get the path to the XML configuration file</summary>
		/// <returns>The file path if available</returns>
		private string GetFileSpec()
		{
			if(m_xmlIni != null)
				return m_xmlIni.FileSpec;
			else
				return "";
			
		}// private string GetFileSpec()
			
		/// <summary>This method is called to set the path to the XML configuration file</summary>
		/// <param name="strFileSpec">The path to the file</param>
		/// <param name="bLoad">true to load the values stored in the file</param>
		/// <returns>true if successful</returns>
		private bool SetFileSpec(string strFileSpec, bool bLoad)
		{
			bool bSuccessful = true;
			
			if((strFileSpec != null) && (strFileSpec.Length > 0))
			{
				if(Open(strFileSpec, bLoad) == true)
					m_strOpened = System.DateTime.Now.ToString();
				else
					bSuccessful = false;
			}
			else
			{
				Close();
			}
			
			return bSuccessful;
			
		}// private bool SetFileSpec(string strFileSpec)
			
		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;
			
			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;
		
			if(aStrings == null) return;
				
			//	The format strings must be added in the order in which they are defined
			//			aStrings.Add("An exception was raised while attempting to save the XML case options: filename = %1");
			//			aStrings.Add("An exception was raised while attempting to initialize the case options: filename = %1");

		}// private void SetErrorStrings()
		
		#endregion Private Methods
		
		#region Properties
		
		/// <summary>The EventSource for this object</summary>
		public CTmaxEventSource EventSource
		{
			get { return m_tmaxEventSource; }
		}
		
		/// <summary>Unique Id assigned to the case</summary>
		public string CaseId
		{
			get { return m_strCaseId; }
			set { SetCaseId(value); }
		
		}// CaseId property
		
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
			get { return m_strUser; }
			set { m_strUser = value; }
		}
		
		/// <summary>Id of the last highlighter selected by the user</summary>
		public long LastHighlighter
		{
			get { return m_lLastHighlighter; }
			set { m_lLastHighlighter = value; }
		}
		
		/// <summary>Id of the last deposition selected by the user</summary>
		public long LastDeposition
		{
			get { return m_lLastDeposition; }
			set { m_lLastDeposition = value; }
		}
		
		/// <summary>Id of the target binder selected by the user</summary>
		public long TargetBinderId
		{
			get { return m_lTargetBinderId; }
			set { m_lTargetBinderId = value; }
		}
		
		/// <summary>Number of seconds per line required to show pause indicator</summary>
		public int PauseThreshold
		{
			get { return m_iPauseThreshold; }
			set { m_iPauseThreshold = value; }
		}

		/// <summary>Last deposition used when adding an objection</summary>
		public string AddObjectionDeposition
		{
			get { return m_strAddObjectionDeposition; }
			set { m_strAddObjectionDeposition = value; }
		}

		/// <summary>Last argument used when adding an objection</summary>
		public string AddObjectionArgument
		{
			get { return m_strAddObjectionArgument; }
			set { m_strAddObjectionArgument = value; }
		}

		/// <summary>Last side used when adding an objection</summary>
		public bool AddObjectionPlaintiff
		{
			get { return m_bAddObjectionPlaintiff; }
			set { m_bAddObjectionPlaintiff = value; }
		}

		/// <summary>Filter descriptor used by the database to construct the SQL filter statement</summary>
		public FTI.Shared.Trialmax.CTmaxFilter AdvancedFilter
		{
			get { return m_tmaxAdvancedFilter; }
		}

		/// <summary>Options used by the database for import operations</summary>
		public FTI.Shared.Trialmax.CTmaxImportOptions ImportOptions
		{
			get { return m_tmaxImportOptions; }
		}

		/// <summary>Options used by the database for export operations</summary>
		public FTI.Shared.Trialmax.CTmaxExportOptions ExportOptions
		{
			get { return m_tmaxExportOptions; }
		}

		/// <summary>Options used by the database for export objection operations</summary>
		public FTI.Shared.Trialmax.CTmaxExportObjections ExportObjectionOptions
		{
			get { return m_tmaxExportObjections; }
		}

		/// <summary>Options used by the database for trip operations</summary>
		public FTI.Shared.Trialmax.CTmaxTrimOptions TrimOptions
		{
			get { return m_tmaxTrimOptions; }
		}

		/// <summary>The path to the configuration file</summary>
		public string FileSpec
		{
			get { return GetFileSpec(); }
			set { SetFileSpec(value, false); }
		}

		#endregion Properties

	}//	CTmaxStationOptions

}// namespace FTI.Shared.Trialmax
