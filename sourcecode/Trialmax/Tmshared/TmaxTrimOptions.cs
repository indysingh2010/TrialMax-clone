using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
    /// <summary>This class encapsulates the options used to trim a Trialmax database</summary>
    public class CTmaxTrimOptions
    {
		#region Constants

		private const string XMLINI_CASE_FOLDER_KEY					= "CaseFolder";
		private const string XMLINI_KEEP_TREATMENTS_KEY				= "KeepTreatments";
		private const string XMLINI_KEEP_SCRIPTS_KEY				= "KeepScripts";
		private const string XMLINI_USE_MASTER_KEY					= "UseMaster";
		private const string XMLINI_FIRST_DOCUMENT_KEY				= "FirstDocument";
		private const string XMLINI_LAST_DOCUMENT_KEY				= "LastDocument";
		private const string XMLINI_KEEP_FIRST_PAGE_KEY				= "KeepFirstPage";
		private const string XMLINI_UNUSED_MEDIA_RECORDS_KEY		= "UnusedMediaRecords";

		#endregion Constants
		
        #region Private Members

		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to CaseId property</summary>
		private string m_strCaseId = "";

		/// <summary>Local member bound to CaseFolder property</summary>
        private string m_strCaseFolder = "";

		/// <summary>Local member bound to KeepTreatments property</summary>
		private bool m_bKeepTreatments = true;

		/// <summary>Local member bound to KeepScripts property</summary>
		private bool m_bKeepScripts = true;

		/// <summary>Local member bound to UseMasterRange property</summary>
		private bool m_bUseMasterRange = true;

		/// <summary>Local member bound to FirstDocument property</summary>
		private long m_lFirstDocument = 0;

		/// <summary>Local member bound to LastDocument property</summary>
		private long m_lLastDocument = 0;

		/// <summary>Local member bound to KeepFirstPage property</summary>
		private bool m_bKeepFirstPage = true;

		/// <summary>Local member bound to UnusedMediaRecords property</summary>
		private bool m_bUnusedMediaRecords = false;

		/// <summary>Local member bound to BinderSelections property</summary>
		private CTmaxItems m_tmaxBinderSelections = new CTmaxItems();
		
		#endregion Private Members

        #region Public Methods

        /// <summary>Constructor</summary>
        public CTmaxTrimOptions()
        {
			//	Set up the event source
			SetErrorStrings();
			m_tmaxEventSource.Name = "Trim Options";

        }// CTmaxTrimOptions()

		/// <summary>Called to reset the properties to their default values</summary>
		public void Clear()
		{
			m_strCaseFolder = "";
			m_bKeepScripts = true;
			m_bKeepTreatments = true;
			m_bUseMasterRange = false;
			m_lFirstDocument = 0;
			m_lLastDocument = 0;
			m_bKeepFirstPage = true;
			m_bUnusedMediaRecords = false;
			m_tmaxBinderSelections.Clear();
		}

		/// <summary>Called to copy the properties of the specified object</summary>
		/// <param name="tmaxSource">the source object</param>
		public void Copy(CTmaxTrimOptions tmaxSource)
		{
			m_strCaseFolder = tmaxSource.m_strCaseFolder;
			m_bKeepScripts = tmaxSource.m_bKeepScripts;
			m_bKeepTreatments = tmaxSource.m_bKeepTreatments;
			m_bUseMasterRange = tmaxSource.m_bUseMasterRange;
			m_bKeepFirstPage = tmaxSource.m_bKeepFirstPage;
			m_bUnusedMediaRecords = tmaxSource.m_bUnusedMediaRecords;
			m_lFirstDocument = tmaxSource.m_lFirstDocument;
			m_lLastDocument = tmaxSource.m_lLastDocument;
		
			m_tmaxBinderSelections.Clear();
			foreach(CTmaxItem O in tmaxSource.BinderSelections)
				m_tmaxBinderSelections.Add(new CTmaxItem(O));
		}

		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the values are stored</param>
		/// <returns>true if successful</returns>
		public bool Load(CXmlIni xmlIni)
		{
			string strSection = "";

			//	Make sure we have a valid section name
			strSection = GetXmlSectionName();
			if (strSection.Length == 0) return false;

			if (xmlIni.SetSection(strSection) == false) return false;

			m_strCaseFolder = xmlIni.Read(XMLINI_CASE_FOLDER_KEY, m_strCaseFolder);
			m_bKeepScripts = xmlIni.ReadBool(XMLINI_KEEP_SCRIPTS_KEY, m_bKeepScripts);
			m_bKeepTreatments = xmlIni.ReadBool(XMLINI_KEEP_TREATMENTS_KEY, m_bKeepTreatments);
			m_bUseMasterRange = xmlIni.ReadBool(XMLINI_USE_MASTER_KEY, m_bUseMasterRange);
			m_bKeepFirstPage = xmlIni.ReadBool(XMLINI_KEEP_FIRST_PAGE_KEY, m_bKeepFirstPage);
			m_bUnusedMediaRecords = xmlIni.ReadBool(XMLINI_UNUSED_MEDIA_RECORDS_KEY, m_bUnusedMediaRecords);
			m_lFirstDocument = xmlIni.ReadLong(XMLINI_FIRST_DOCUMENT_KEY, m_lFirstDocument);
			m_lLastDocument = xmlIni.ReadLong(XMLINI_LAST_DOCUMENT_KEY, m_lLastDocument);
		
			return true;

		}// private bool Load(CXmlIni xmlIni)

		/// <summary>This method is called to store the application options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file to store the application option values</param>
		/// <returns>true if successful</returns>
		public bool Save(CXmlIni xmlIni)
		{
			string strSection = "";

			//	Make sure we have a valid section name
			strSection = GetXmlSectionName();
			if (strSection.Length == 0) return false;

			if (xmlIni.SetSection(strSection, true) == false) return false;

			xmlIni.Write(XMLINI_CASE_FOLDER_KEY, m_strCaseFolder);
			xmlIni.Write(XMLINI_KEEP_SCRIPTS_KEY, m_bKeepScripts);
			xmlIni.Write(XMLINI_KEEP_TREATMENTS_KEY, m_bKeepTreatments);
			xmlIni.Write(XMLINI_USE_MASTER_KEY, m_bUseMasterRange);
			xmlIni.Write(XMLINI_KEEP_FIRST_PAGE_KEY, m_bKeepFirstPage);
			xmlIni.Write(XMLINI_UNUSED_MEDIA_RECORDS_KEY, m_bUnusedMediaRecords);
			xmlIni.Write(XMLINI_FIRST_DOCUMENT_KEY, m_lFirstDocument);
			xmlIni.Write(XMLINI_LAST_DOCUMENT_KEY, m_lLastDocument);
			return true;

		}// private bool Save(CXmlIni xmlIni)

		#endregion Public Methods

        #region Private Methods

		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		private string GetXmlSectionName()
		{
			if (m_strCaseId.Length > 0)
				return ("trialMax/station/trim/" + m_strCaseId);
			else
				return "";

		}// private string GetXmlSectionName()

		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;

			if (m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;

			if (aStrings == null) return;

			//	The format strings must be added in the order in which they are defined
			//			aStrings.Add("An exception was raised while attempting to open the XML case options: filename = %1");
			//			aStrings.Add("Unable to save the case options. The XML file has not been opened.");
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
			set { m_strCaseId = value; }

		}// CaseId property

		/// <ssummary>Folder where trimmed case is located</summary>
        public string CaseFolder
        {
            get { return m_strCaseFolder; }
            set { m_strCaseFolder = value; }
        }

		/// <ssummary>True to keep documents that have treated pages</summary>
		public bool KeepTreatments
		{
			get { return m_bKeepTreatments; }
			set { m_bKeepTreatments = value; }
		}

		/// <ssummary>True to keep documents that appear in scripts</summary>
		public bool KeepScripts
		{
			get { return m_bKeepScripts; }
			set { m_bKeepScripts = value; }
		}

		/// <ssummary>True to use master range to determine page transfers</summary>
		public bool UseMasterRange
		{
			get { return m_bUseMasterRange; }
			set { m_bUseMasterRange = value; }
		}

		/// <ssummary>True to include unused media records</summary>
		public bool UnusedMediaRecords
		{
			get { return m_bUnusedMediaRecords; }
			set { m_bUnusedMediaRecords = value; }
		}

		/// <ssummary>Id of first document in the master range</summary>
		public long FirstDocument
		{
			get { return m_lFirstDocument; }
			set { m_lFirstDocument = value; }
		}

		/// <ssummary>Id of last document in the master range</summary>
		public long LastDocument
		{
			get { return m_lLastDocument; }
			set { m_lLastDocument = value; }
		}

		/// <ssummary>True to always keep first page of a document</summary>
		public bool KeepFirstPage
		{
			get { return m_bKeepFirstPage; }
			set { m_bKeepFirstPage = value; }
		}

		/// <ssummary>Collection of event items to represent selected binders</summary>
		public CTmaxItems BinderSelections
		{
			get { return m_tmaxBinderSelections; }
		}

		#endregion Properties

    }//	public class CTmaxTrimOptions

}// namespace FTI.Shared.Trialmax
