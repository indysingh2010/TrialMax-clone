using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Trialmax.Reports;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Reports
{
	/// <summary>This class encapsulates the options used to generate a playlist report</summary>
	public class CROExhibits : CROBase
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "ExhibitsReportOptions";
		private const string XMLINI_INCLUDE_SUB_BINDERS_KEY		= "IncludeSubBinders";
		private const string XMLINI_INCLUDE_PAGES_KEY			= "IncludePages";
		private const string XMLINI_INCLUDE_TREATMENTS_KEY		= "IncludeTreatments";
		private const string XMLINI_INCLUDE_ONLY_MAPPED_KEY		= "IncludeOnlyMapped";
		private const string XMLINI_INCLUDE_ONLY_ADMITTED_KEY	= "IncludeOnlyAdmitted";
		private const string XMLINI_SORT_ORDER_KEY				= "SortOrder";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to IncludeSubBinders property</summary>
		private bool m_bIncludeSubBinders = true;
		
		/// <summary>Local member bound to IncludePages property</summary>
		private bool m_bIncludePages = false;
		
		/// <summary>Local member bound to IncludeTreatments property</summary>
		private bool m_bIncludeTreatments = false;

		/// <summary>Local member bound to IncludeOnlyMapped property</summary>
		private bool m_bIncludeOnlyMapped = false;

		/// <summary>Local member bound to IncludeOnlyAdmitted property</summary>
		private bool m_bIncludeOnlyAdmitted = false;

		/// <summary>Local member bound to SubTitle property</summary>
		private string m_strSubTitle = "";

		/// <summary>Local member bound to SortOrder property</summary>
		private int m_iSortOrder = 0;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CROExhibits()
		{
		
		}// CROExhibits()
		
		/// <summary>This method is called to get the XML section name</summary>
		/// <returns>The section containing all the option values</returns>
		/// <remarks>Derived classes should override this method to make sure they use a unique section name</remarks>
		public override string GetXmlSectionName()
		{
			return XMLINI_SECTION_NAME;
		}
		
		/// <summary>This method is called to get name of the default XML schema file</summary>
		/// <returns>The default schema filename</returns>
		/// <remarks>Derived classes should override this method to make sure they provide the appropriate name</remarks>
		public override string GetDefaultXmlSchema()
		{
			return "_tmax_report_exhibit.xsd";
		}
		
		/// <summary>This method is called to populate the templates collection with the defaults for the report</summary>
		protected override void GetDefaultTemplates()
		{
			m_aTemplates.Add(new CROTemplate("_tmax_report_exhibit.rpt", GetDefaultXmlSchema(), "Standard Exhibits List"));
			m_aTemplates.Add(new CROTemplate("_tmax_report_exhibit_col.rpt", GetDefaultXmlSchema(), "Condensed Exhibits List"));
			m_aTemplates.Add(new CROTemplate("_tmax_report_exhibit_excel.rpt", GetDefaultXmlSchema(), "Exhibits Spreadsheet"));
			m_aTemplates.Add(new CROTemplate("_tmax_report_exhibit_fbc.rpt", GetDefaultXmlSchema(), "Foreign Barcodes List"));
		}
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public override void Load(CXmlIni xmlIni)
		{
			//	Do the base class processing first
			base.Load(xmlIni);
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_bIncludeSubBinders = xmlIni.ReadBool(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			m_bIncludePages = xmlIni.ReadBool(XMLINI_INCLUDE_PAGES_KEY, m_bIncludePages);
			m_bIncludeTreatments = xmlIni.ReadBool(XMLINI_INCLUDE_TREATMENTS_KEY, m_bIncludeTreatments);
			m_bIncludeOnlyMapped = xmlIni.ReadBool(XMLINI_INCLUDE_ONLY_MAPPED_KEY, m_bIncludeOnlyMapped);
			m_bIncludeOnlyAdmitted = xmlIni.ReadBool(XMLINI_INCLUDE_ONLY_ADMITTED_KEY, m_bIncludeOnlyAdmitted);
			m_iSortOrder = xmlIni.ReadInteger(XMLINI_SORT_ORDER_KEY, m_iSortOrder);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public override void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			xmlIni.Write(XMLINI_INCLUDE_PAGES_KEY, m_bIncludePages);
			xmlIni.Write(XMLINI_INCLUDE_TREATMENTS_KEY, m_bIncludeTreatments);
			xmlIni.Write(XMLINI_INCLUDE_ONLY_MAPPED_KEY, m_bIncludeOnlyMapped);
			xmlIni.Write(XMLINI_INCLUDE_ONLY_ADMITTED_KEY, m_bIncludeOnlyAdmitted);
			xmlIni.Write(XMLINI_SORT_ORDER_KEY, m_iSortOrder);

			//	Do the base class processing last
			base.Save(xmlIni);
			
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Properties
			
		/// <summary>Include scripts contained in subbinders</summary>
		public bool IncludeSubBinders
		{
			get { return m_bIncludeSubBinders; }
			set { m_bIncludeSubBinders = value; }
		}
		
		/// <summary>Include pages (page/slide/segments) in the report</summary>
		public bool IncludePages
		{
			get { return m_bIncludePages; }
			set { m_bIncludePages = value; }
		}
		
		/// <summary>Include treatments in the report</summary>
		public bool IncludeTreatments
		{
			get { return m_bIncludeTreatments; }
			set { m_bIncludeTreatments = value; }
		}
		
		/// <summary>Include only admitted records</summary>
		public bool IncludeOnlyAdmitted
		{
			get { return m_bIncludeOnlyAdmitted; }
			set { m_bIncludeOnlyAdmitted = value; }
		}
		
		/// <summary>Exclude unmapped records in the report</summary>
		public bool IncludeOnlyMapped
		{
			get { return m_bIncludeOnlyMapped; }
			set { m_bIncludeOnlyMapped = value; }
		}
		
		/// <summary>Sort order identifier</summary>
		public int SortOrder
		{
			get { return m_iSortOrder; }
			set { m_iSortOrder = value; }
		}
		
		/// <summary>Subtitle to appear in the report</summary>
		public string SubTitle
		{
			get { return m_strSubTitle; }
			set { m_strSubTitle = value; }
		}
		
		#endregion Properties

	}//	public class CROExhibits

}// namespace FTI.Trialmax.Reports
