using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Shared.Xml;
using FTI.Shared.Trialmax;
using FTI.Trialmax.Reports;

namespace FTI.Trialmax.Reports
{
	/// <summary>This class manages the options used to generate an objections report</summary>
	public class CROObjections : CROBase
	{
		#region Constants

		private const string XMLINI_SECTION_NAME = "ObjectionReportOptions";
		private const string XMLINI_INCLUDE_SUB_BINDERS_KEY = "IncludeSubBinders";
		private const string XMLINI_CASE_ID_KEY = "CaseId";
		private const string XMLINI_FILTER_INDEX_KEY = "FilterIndex";

		#endregion Constants

		#region Private Members

		/// <summary>Local member bound to IncludeSubBinders property</summary>
		private bool m_bIncludeSubBinders = true;

		/// <summary>Local member bound to CaseId property</summary>
		private string m_strCaseId = "";

		/// <summary>Local member bound to FilterIndex property</summary>
		private int m_iFilterIndex = 0;

		/// <summary>The collection of ORE templates identified by the standard report templates</summary>
		private FTI.Shared.Trialmax.CTmaxORETemplates m_tmaxORETemplates = new CTmaxORETemplates();

		/// <summary>The ORE template used to exchange data with the application's configuration file</summary>
		private FTI.Shared.Trialmax.CTmaxORETemplate m_lastORETemplate = null;

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CROObjections()
		{

		}// CROObjections()

		/// <summary>This method is called to get the XML section name</summary>
		/// <returns>The section containing all the option values</returns>
		/// <remarks>Derived classes should override this method to make sure they use a unique section name</remarks>
		public override string GetXmlSectionName()
		{
			return XMLINI_SECTION_NAME;
		}

		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public override void Load(CXmlIni xmlIni)
		{
			int		iTemplate = 1;
			string	strKey = "";
			string	strName = "";
			string	strFilename = "";
			string	strSchema = "";

			//	We don't do the base class processing for this class
			//base.Load(xmlIni);

			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;

			m_bIncludeSubBinders = xmlIni.ReadBool(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			m_iTemplate = xmlIni.ReadInteger(XMLINI_TEMPLATE_KEY, m_iTemplate);
			m_bAddMediaName = xmlIni.ReadBool(XMLINI_ADD_MEDIA_NAME_KEY, m_bAddMediaName);
			m_strExportFolder = xmlIni.Read(XMLINI_EXPORT_FOLDER_KEY, m_strExportFolder);
			m_strCaseId = xmlIni.Read(XMLINI_CASE_ID_KEY, m_strCaseId);
			m_iFilterIndex = xmlIni.ReadInteger(XMLINI_FILTER_INDEX_KEY, m_iFilterIndex);

			//	Read the template descriptors
			while(true)
			{
				//	Construct the key
				strKey = String.Format("Template{0}", iTemplate);
				iTemplate++;

				//	Read the template attributes
				strFilename = xmlIni.Read(strKey, XMLINI_TEMPLATE_FILENAME_ATTRIBUTE, "");
				strSchema = xmlIni.Read(strKey, XMLINI_TEMPLATE_SCHEMA_ATTRIBUTE, GetDefaultXmlSchema());
				strName = xmlIni.Read(strKey, XMLINI_TEMPLATE_NAME_ATTRIBUTE, "");

				if(strName.Length > 0)
				{
					m_aTemplates.Add(new CROTemplate(strFilename, strSchema, strName));
				}
				else
				{
					break;
				}

			}// while(true)

			//	Add the default template if none found in the file
			if(m_aTemplates.Count == 0)
			{
				m_aTemplates.Add(new CROTemplate("", "Excel.xls", "Excel Spreadsheet"));
				m_aTemplates.Add(new CROTemplate("0", "DesigWord.doc", "Word Table"));
			}

			//	Load the last template information
			m_lastORETemplate = new CTmaxORETemplate();

			xmlIni.SetSection(GetXmlSectionName() + "\\" + m_lastORETemplate.Common.Name);
			m_lastORETemplate.Common.Load(xmlIni);

			xmlIni.SetSection(GetXmlSectionName() + "\\" + m_lastORETemplate.Flags.Name);
			m_lastORETemplate.Flags.Load(xmlIni);

		}// public void Load(CXmlIni xmlIni)

		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public override void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;

			xmlIni.Write(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			xmlIni.Write(XMLINI_CASE_ID_KEY, m_strCaseId);
			xmlIni.Write(XMLINI_FILTER_INDEX_KEY, m_iFilterIndex);
			
			//	Save the information for the last template
			if(m_lastORETemplate != null)
			{
				xmlIni.SetSection(GetXmlSectionName() + "\\" + m_lastORETemplate.Common.Name);
				m_lastORETemplate.Common.Save(xmlIni);

				xmlIni.SetSection(GetXmlSectionName() + "\\" + m_lastORETemplate.Flags.Name);
				m_lastORETemplate.Flags.Save(xmlIni);
			}

			//	Do the base class processing last
			base.Save(xmlIni);

		}// public void Save(CXmlIni xmlIni)

		/// <summary>This method is called to add an ORE template to the collection</summary>
		/// <param name="tmaxTemplate">The template to be added</param>
		/// <returns>True if successful</returns>
		public bool AddORETemplate(CTmaxORETemplate tmaxORETemplate)
		{
			CTmaxORETemplate tmaxDuplicate = null;
			
			//	Make sure we don't have a duplicate
			if((tmaxDuplicate = GetORETemplate(tmaxORETemplate.Name)) != null)
			{
				//	Should we remove the duplicate
				if(ReferenceEquals(tmaxDuplicate, tmaxORETemplate) == false)
				{
					m_tmaxORETemplates.Remove(tmaxDuplicate);
				}
				else
				{
					//	Prevent adding to the collection again
					tmaxORETemplate = null;
				}

			}// if((tmaxDuplicate = GetORETemplate(tmaxORETemplate)) != null)
			
			if(tmaxORETemplate != null)
			{
				m_tmaxORETemplates.Add(tmaxORETemplate);
			}
			
			return m_tmaxORETemplates.Contains(tmaxORETemplate);

		}// public bool AddORETemplate(CTmaxORETemplate tmaxORETemplate)
		
		/// <summary>This method is called to get the ORE template associated with the specified TrialMax report template</summary>
		/// <param name="tmaxTemplate">The template to be matched</param>
		/// <returns>The associated ORE template if found</returns>
		public FTI.Shared.Trialmax.CTmaxORETemplate GetORETemplate(CROTemplate tmaxTemplate)
		{
			return GetORETemplate(tmaxTemplate.Name);
		}

		/// <summary>This method is called to get the ORE template with the specified name</summary>
		/// <param name="strName">The name of the desired template</param>
		/// <returns>The desired ORE template if found</returns>
		public FTI.Shared.Trialmax.CTmaxORETemplate GetORETemplate(string strName)
		{
			if((m_tmaxORETemplates != null) && (m_tmaxORETemplates.Count > 0))
			{
				foreach(CTmaxORETemplate O in m_tmaxORETemplates)
				{
					if(String.Compare(O.Name, strName, true) == 0)
						return O;
				}

			}// if((m_tmaxORETemplates != null) && (m_tmaxORETemplates.Count > 0))
			
			return null; // Not found

		}// public FTI.Shared.Trialmax.CTmaxORETemplate GetORETemplate(string strName)

		#endregion Public Methods

		#region Properties

		/// <summary>The MasterId of the active case database</summary>
		public string CaseId
		{
			get { return m_strCaseId; }
			set { m_strCaseId = value; }
		}

		/// <summary>The index of the selection in the Filter drop list</summary>
		public int FilterIndex
		{
			get { return m_iFilterIndex; }
			set { m_iFilterIndex = value; }
		}

		/// <summary>Include scripts contained in subbinders</summary>
		public bool IncludeSubBinders
		{
			get { return m_bIncludeSubBinders; }
			set { m_bIncludeSubBinders = value; }
		}

		/// <summary>The collection of ORE templates</summary>
		public CTmaxORETemplates ORETemplates
		{
			get { return m_tmaxORETemplates; }
		}

		/// <summary>The template used to store and retrieve values in the application configuration file</summary>
		public CTmaxORETemplate LastORETemplate
		{
			get { return m_lastORETemplate; }
			set { m_lastORETemplate = value; }
		}

		#endregion Properties

	}//	public class CROObjections

}// namespace FTI.Trialmax.Reports
