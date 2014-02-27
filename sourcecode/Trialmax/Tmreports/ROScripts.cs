using System;
using System.Collections;
using System.Windows.Forms;

using FTI.Shared;
using FTI.Trialmax.Reports;
using FTI.Shared.Xml;

namespace FTI.Trialmax.Reports
{
	/// <summary>This class encapsulates the options used to generate a playlist report</summary>
	public class CROScripts : CROBase
	{
		#region Constants
		
		private const string XMLINI_SECTION_NAME				= "ScriptsReportOptions";
		private const string XMLINI_INCLUDE_SUB_BINDERS_KEY		= "IncludeSubBinders";
		private const string XMLINI_INCLUDE_PLAYLISTS_KEY		= "IncludePlaylists";
		private const string XMLINI_INCLUDE_CUSTOM_SHOWS_KEY	= "IncludeCustomShows";
		private const string XMLINI_INCLUDE_TRANSCRIPT_TEXT_KEY	= "IncludeTranscriptText";
		private const string XMLINI_PAGE_BREAK_KEY				= "PageBreak";
		private const string XMLINI_HIGHLIGHT_TEXT_KEY			= "HighlightText";
		private const string XMLINI_INDICATE_LINKS_KEY			= "IndicateLinks";
		private const string XMLINI_INDICATE_EDITED_KEY			= "IndicateEdited";
		private const string XMLINI_BARCODE_GRAPHIC_KEY			= "BarcodeGraphic";
		private const string XMLINI_BARCODE_TEXT_KEY			= "BarcodeText";
		private const string XMLINI_MEDIA_FILE_KEY				= "MediaFile";
		private const string XMLINI_DURATION_KEY				= "Duration";
		private const string XMLINI_REMAINING_KEY				= "Remaining";
		private const string XMLINI_ELAPSED_KEY					= "Elapsed";
		private const string XMLINI_TIME_TOTALS_KEY				= "TimeTotals";
		private const string XMLINI_SCENE_NUMBER_KEY			= "SceneNumber";
		
		#endregion Constants
		
		#region Private Members

		/// <summary>Local member bound to SceneNumber property</summary>
		private bool m_bSceneNumber = true;

		/// <summary>Local member bound to BarcodeText property</summary>
		private bool m_bBarcodeText = true;
		
		/// <summary>Local member bound to BarcodeGraphic property</summary>
		private bool m_bBarcodeGraphic = false;
		
		/// <summary>Local member bound to MediaFile property</summary>
		private bool m_bMediaFile = true;
		
		/// <summary>Local member bound to Duration property</summary>
		private bool m_bDuration = true;
		
		/// <summary>Local member bound to Elapsed property</summary>
		private bool m_bElapsed = true;
		
		/// <summary>Local member bound to Remaining property</summary>
		private bool m_bRemaining = true;
		
		/// <summary>Local member bound to TimeTotals property</summary>
		private bool m_bTimeTotals = true;
		
		/// <summary>Local member bound to IncludeSubBinders property</summary>
		private bool m_bIncludeSubBinders = true;
		
		/// <summary>Local member bound to IncludeCustomShows property</summary>
		private bool m_bIncludeCustomShows = true;
		
		/// <summary>Local member bound to IncludePlaylists property</summary>
		private bool m_bIncludePlaylists = true;
		
		/// <summary>Local member bound to IncludeTranscriptText property</summary>
		private bool m_bIncludeTranscriptText = true;

		/// <summary>Local member bound to PageBreak property</summary>
		private bool m_bPageBreak = true;

		/// <summary>Local member bound to HighlightText property</summary>
		private bool m_bHighlightText = true;

		/// <summary>Local member bound to IndicateLinks property</summary>
		private bool m_bIndicateLinks = true;

		/// <summary>Local member bound to IndicateEdited property</summary>
		private bool m_bIndicateEdited = true;

		#endregion Private Members
		
		#region Public Methods
		
		/// <summary>Constructor</summary>
		public CROScripts()
		{
		
		}// CROScripts()
		
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
			return "_tmax_report_script_multiple.xsd";
		}
		
		/// <summary>This method is called to populate the templates collection with the defaults for the report</summary>
		protected override void GetDefaultTemplates()
		{
			m_aTemplates.Add(new CROTemplate("_tmax_report_script_multiple.rpt", GetDefaultXmlSchema(), "Standard"));
		}
		
		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public override void Load(CXmlIni xmlIni)
		{
			//	Do the base class processing first
			base.Load(xmlIni);
			
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;
			
			m_bIncludeSubBinders = xmlIni.ReadBool(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			m_bIncludeCustomShows = xmlIni.ReadBool(XMLINI_INCLUDE_CUSTOM_SHOWS_KEY, m_bIncludeCustomShows);
			m_bIncludePlaylists = xmlIni.ReadBool(XMLINI_INCLUDE_PLAYLISTS_KEY, m_bIncludePlaylists);
			m_bIncludeTranscriptText = xmlIni.ReadBool(XMLINI_INCLUDE_TRANSCRIPT_TEXT_KEY, m_bIncludeTranscriptText);
			m_bPageBreak = xmlIni.ReadBool(XMLINI_PAGE_BREAK_KEY, m_bPageBreak);
			m_bHighlightText = xmlIni.ReadBool(XMLINI_HIGHLIGHT_TEXT_KEY, m_bHighlightText);
			m_bIndicateLinks = xmlIni.ReadBool(XMLINI_INDICATE_LINKS_KEY, m_bIndicateLinks);
			m_bIndicateEdited = xmlIni.ReadBool(XMLINI_INDICATE_EDITED_KEY, m_bIndicateEdited);
			m_bBarcodeGraphic = xmlIni.ReadBool(XMLINI_BARCODE_GRAPHIC_KEY, m_bBarcodeGraphic);
			m_bBarcodeText = xmlIni.ReadBool(XMLINI_BARCODE_TEXT_KEY, m_bBarcodeText);
			m_bMediaFile = xmlIni.ReadBool(XMLINI_MEDIA_FILE_KEY, m_bMediaFile);
			m_bElapsed = xmlIni.ReadBool(XMLINI_ELAPSED_KEY, m_bElapsed);
			m_bRemaining = xmlIni.ReadBool(XMLINI_REMAINING_KEY, m_bRemaining);
			m_bDuration = xmlIni.ReadBool(XMLINI_DURATION_KEY, m_bDuration);
			m_bTimeTotals = xmlIni.ReadBool(XMLINI_TIME_TOTALS_KEY, m_bTimeTotals);
			m_bSceneNumber = xmlIni.ReadBool(XMLINI_SCENE_NUMBER_KEY, m_bSceneNumber);
			
		}// public void Load(CXmlIni xmlIni)
		
		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public override void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(XMLINI_SECTION_NAME) == false) return;

			xmlIni.Write(XMLINI_INCLUDE_SUB_BINDERS_KEY, m_bIncludeSubBinders);
			xmlIni.Write(XMLINI_INCLUDE_CUSTOM_SHOWS_KEY, m_bIncludeCustomShows);
			xmlIni.Write(XMLINI_INCLUDE_PLAYLISTS_KEY, m_bIncludePlaylists);
			xmlIni.Write(XMLINI_INCLUDE_TRANSCRIPT_TEXT_KEY, m_bIncludeTranscriptText);
			xmlIni.Write(XMLINI_PAGE_BREAK_KEY, m_bPageBreak);
			xmlIni.Write(XMLINI_HIGHLIGHT_TEXT_KEY, m_bHighlightText);
			xmlIni.Write(XMLINI_INDICATE_LINKS_KEY, m_bIndicateLinks);
			xmlIni.Write(XMLINI_INDICATE_EDITED_KEY, m_bIndicateEdited);
			xmlIni.Write(XMLINI_BARCODE_GRAPHIC_KEY, m_bBarcodeGraphic);
			xmlIni.Write(XMLINI_ALTERNATE_KEY, m_strAlternate);
			xmlIni.Write(XMLINI_BARCODE_TEXT_KEY, m_bBarcodeText);
			xmlIni.Write(XMLINI_MEDIA_FILE_KEY, m_bMediaFile);
			xmlIni.Write(XMLINI_ELAPSED_KEY, m_bElapsed);
			xmlIni.Write(XMLINI_REMAINING_KEY, m_bRemaining);
			xmlIni.Write(XMLINI_DURATION_KEY, m_bDuration);
			xmlIni.Write(XMLINI_TIME_TOTALS_KEY, m_bTimeTotals);
			xmlIni.Write(XMLINI_SCENE_NUMBER_KEY, m_bSceneNumber);
			
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
		
		/// <summary>Include custom show scripts</summary>
		public bool IncludeCustomShows
		{
			get { return m_bIncludeCustomShows; }
			set { m_bIncludeCustomShows = value; }
		}
		
		/// <summary>Include playlists</summary>
		public bool IncludePlaylists
		{
			get { return m_bIncludePlaylists; }
			set { m_bIncludePlaylists = value; }
		}
		
		/// <summary>Include transcript text</summary>
		public bool IncludeTranscriptText
		{
			get { return m_bIncludeTranscriptText; }
			set { m_bIncludeTranscriptText = value; }
		}
		
		/// <summary>Highlight text</summary>
		public bool HighlightText
		{
			get { return m_bHighlightText; }
			set { m_bHighlightText = value; }
		}
		
		/// <summary>Show link indicators</summary>
		public bool IndicateLinks
		{
			get { return m_bIndicateLinks; }
			set { m_bIndicateLinks = value; }
		}
		
		/// <summary>Show edited indicators</summary>
		public bool IndicateEdited
		{
			get { return m_bIndicateEdited; }
			set { m_bIndicateEdited = value; }
		}
		
		/// <summary>Break page after each script</summary>
		public bool PageBreak
		{
			get { return m_bPageBreak; }
			set { m_bPageBreak = value; }
		}
		
		/// <summary>Show the graphic for the barcode</summary>
		public bool BarcodeGraphic
		{
			get { return m_bBarcodeGraphic; }
			set { m_bBarcodeGraphic = value; }
		}
		
		/// <summary>Show the text for the barcode</summary>
		public bool BarcodeText
		{
			get { return m_bBarcodeText; }
			set { m_bBarcodeText = value; }
		}
		
		/// <summary>Show the media filename</summary>
		public bool MediaFile
		{
			get { return m_bMediaFile; }
			set { m_bMediaFile = value; }
		}
		
		/// <summary>Show the duration</summary>
		public bool Duration
		{
			get { return m_bDuration; }
			set { m_bDuration = value; }
		}
		
		/// <summary>Show the elapsed time</summary>
		public bool Elapsed
		{
			get { return m_bElapsed; }
			set { m_bElapsed = value; }
		}
		
		/// <summary>Show the remaining time</summary>
		public bool Remaining
		{
			get { return m_bRemaining; }
			set { m_bRemaining = value; }
		}
		
		/// <summary>Show the time totals</summary>
		public bool TimeTotals
		{
			get { return m_bTimeTotals; }
			set { m_bTimeTotals = value; }
		}

		/// <summary>Show the scene number</summary>
		public bool SceneNumber
		{
			get { return m_bSceneNumber; }
			set { m_bSceneNumber = value; }
		}

		#endregion Properties

	}//	public class CROScripts

}// namespace FTI.Trialmax.Reports
