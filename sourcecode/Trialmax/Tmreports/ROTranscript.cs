using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Trialmax.Reports;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Reports
{
	/// <summary>This class encapsulates the options used to generate a transcript report</summary>
	public class CROTranscript : CROBase
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "TranscriptReportOptions";
		private const string XMLINI_BOLD_DESIGNATIONS_KEY		= "BoldDesignations";
		private const string XMLINI_VERTICAL_HIGHLIGHTS_KEY		= "VerticalHighlights";
		private const string XMLINI_PAGE_HEADER_FIRST_KEY		= "PageHeaderFirst";
		private const string XMLINI_PAGE_HEADER_SUBSEQUENT_KEY	= "PageHeaderSubsequent";
		private const string XMLINI_PAGE_FOOTER_KEY				= "PageFooter";
		private const string XMLINI_MEDIUM_FONT_KEY				= "MediumFont";
		
		#endregion Constants
		
		#region Private Members
		
		/// <summary>Local member bound to BoldDesignations property</summary>
		private bool m_bBoldDesignations = true;
		
		/// <summary>Local member bound to VerticalHighlights property</summary>
		private bool m_bVerticalHighlights = true;

		/// <summary>Local member bound to PageHeaderFirst property</summary>
		private bool m_bPageHeaderFirst = true;

		/// <summary>Local member bound to PageHeaderSubsequent property</summary>
		private bool m_bPageHeaderSubsequent = true;

		/// <summary>Local member bound to MediumFont property</summary>
		private bool m_bMediumFont = true;

		/// <summary>Local member bound to PageFooter property</summary>
		private bool m_bPageFooter = true;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CROTranscript()
		{
		
		}// CROTranscript()
		
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
			return "_tmax_report_transcript.xsd";
		}
		
		/// <summary>This method is called to populate the templates collection with the defaults for the report</summary>
		protected override void GetDefaultTemplates()
		{
			m_aTemplates.Add(new CROTemplate("_tmax_report_transcript_minuscript.rpt", GetDefaultXmlSchema(), "Minuscript"));
			m_aTemplates.Add(new CROTemplate("_tmax_report_transcript_manuscript.rpt", GetDefaultXmlSchema(), "Manuscript"));
		}
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public override void Load(CXmlIni xmlIni)
		{
			//	Do the base class processing first
			base.Load(xmlIni);
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_bBoldDesignations = xmlIni.ReadBool(XMLINI_BOLD_DESIGNATIONS_KEY, m_bBoldDesignations);
			m_bVerticalHighlights = xmlIni.ReadBool(XMLINI_VERTICAL_HIGHLIGHTS_KEY, m_bVerticalHighlights);
			m_bPageHeaderFirst = xmlIni.ReadBool(XMLINI_PAGE_HEADER_FIRST_KEY, m_bPageHeaderFirst);
			m_bPageHeaderSubsequent = xmlIni.ReadBool(XMLINI_PAGE_HEADER_SUBSEQUENT_KEY, m_bPageHeaderSubsequent);
			m_bPageFooter = xmlIni.ReadBool(XMLINI_PAGE_FOOTER_KEY, m_bPageFooter);
			m_bMediumFont = xmlIni.ReadBool(XMLINI_MEDIUM_FONT_KEY, m_bMediumFont);
			m_strAlternate = xmlIni.Read(XMLINI_ALTERNATE_KEY, m_strAlternate);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public override void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_BOLD_DESIGNATIONS_KEY, m_bBoldDesignations);
			xmlIni.Write(XMLINI_VERTICAL_HIGHLIGHTS_KEY, m_bVerticalHighlights);
			xmlIni.Write(XMLINI_PAGE_HEADER_FIRST_KEY, m_bPageHeaderFirst);
			xmlIni.Write(XMLINI_PAGE_HEADER_SUBSEQUENT_KEY, m_bPageHeaderSubsequent);
			xmlIni.Write(XMLINI_PAGE_FOOTER_KEY, m_bPageFooter);
			xmlIni.Write(XMLINI_MEDIUM_FONT_KEY, m_bMediumFont);
			xmlIni.Write(XMLINI_ALTERNATE_KEY, m_strAlternate);

			//	Do the base class processing last
			base.Save(xmlIni);
			
		}// public void Save(CXmlIni xmlIni)
		
		#endregion Public Methods
		
		#region Properties
		
		/// <summary>Bold text for designations</summary>
		public bool BoldDesignations
		{
			get { return m_bBoldDesignations; }
			set { m_bBoldDesignations = value; }
		}
		
		/// <summary>Vertical highlight bars</summary>
		public bool VerticalHighlights
		{
			get { return m_bVerticalHighlights; }
			set { m_bVerticalHighlights = value; }
		}
		
		/// <summary>Page header on all subsequent pages</summary>
		public bool PageHeaderSubsequent
		{
			get { return m_bPageHeaderSubsequent; }
			set { m_bPageHeaderSubsequent = value; }
		}
		
		/// <summary>Use media font</summary>
		public bool MediumFont
		{
			get { return m_bMediumFont; }
			set { m_bMediumFont = value; }
		}
		
		/// <summary>Insert page footer</summary>
		public bool PageFooter
		{
			get { return m_bPageFooter; }
			set { m_bPageFooter = value; }
		}
		
		/// <summary>Include header on first page</summary>
		public bool PageHeaderFirst
		{
			get { return m_bPageHeaderFirst; }
			set { m_bPageHeaderFirst = value; }
		}
		
		#endregion Properties

	}//	public class CROTranscript

}// namespace FTI.Trialmax.Reports
