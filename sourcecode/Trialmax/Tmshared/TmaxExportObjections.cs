using System;
using System.Collections;
using System.Windows.Forms;
using System.Text;

using FTI.Shared;
using FTI.Shared.Trialmax;
using FTI.Shared.Xml;

namespace FTI.Shared.Trialmax
{
	/// <summary>This class encapsulates the options used to export records</summary>
	public class CTmaxExportObjections
	{
		#region Constants

		private const string XMLINI_DELIMITER_KEY = "Delimiter";
		private const string XMLINI_ADD_QUOTES_KEY = "AddQuotes";
		private const string XMLINI_SUB_BINDERS_KEY = "SubBinders";
		private const string XMLINI_CRLF_REPLACEMENT_KEY = "CRLFReplacement";
		private const string XMLINI_USER_CRLF_REPLACEMENT_KEY = "UserCRLF";

		#endregion Constants

		#region Private Members

		/// <summary>Error builder object used to construct formatted error messages</summary>
		private CTmaxErrorBuilder m_tmaxErrorBuilder = new CTmaxErrorBuilder();

		/// <summary>Local member bound to EventSource property</summary>
		private CTmaxEventSource m_tmaxEventSource = new CTmaxEventSource();

		/// <summary>Local member bound to Name property</summary>
		private string m_strName = "Objections";

		/// <summary>Local member bound to Delimiter property</summary>
		private TmaxExportDelimiters m_eDelimiter = TmaxExportDelimiters.Tab;

		/// <summary>Local member bound to CRLFReplacement property</summary>
		private TmaxExportCRLF m_eCRLFReplacement = TmaxExportCRLF.Pipe;

		/// <summary>Local member bound to AddQuotes property</summary>
		private bool m_bAddQuotes = false;

		/// <summary>Local member bound to SubBinders property</summary>
		private bool m_bSubBinders = false;

		/// <summary>Local member bound to UserCRLF property</summary>
		private string m_strUserCRLF = "";

		#endregion Private Members

		#region Public Methods

		/// <summary>Constructor</summary>
		public CTmaxExportObjections()
		{
			//	Peform one time initialization
			Initialize();
		}

		/// <summary>Copy constructor</summary>
		/// <param name="tmaxOptions">The source object to copy</param>
		public CTmaxExportObjections(CTmaxExportObjections tmaxSource)
		{
			//	Peform one time initialization
			Initialize();

			if(tmaxSource != null)
				Copy(tmaxSource);

		}// public CTmaxExportObjections(CTmaxExportObjections tmaxSource)

		/// <summary>This method copies the source object</summary>
		/// <param name="tmaxOptions">The source object to copy</param>
		public void Copy(CTmaxExportObjections tmaxSource)
		{
			this.Name = tmaxSource.Name;
			this.AddQuotes = tmaxSource.AddQuotes;
			this.Delimiter = tmaxSource.Delimiter;
			this.SubBinders = tmaxSource.SubBinders;
			this.CRLFReplacement = tmaxSource.CRLFReplacement;
			this.UserCRLF = tmaxSource.UserCRLF;
		}

		/// <summary>This method resets the object to its original state</summary>
		public void Clear()
		{
			m_eDelimiter = TmaxExportDelimiters.Tab;
			m_bAddQuotes = false;
			m_bSubBinders = false;
			m_strUserCRLF = "";

		}// public void Clear()

		/// <summary>This method is called to get the name of the section in the XML configuration file for this filter</summary>
		/// <returns>The name of the section containing the configuration information for this filter</returns>
		public string GetXmlSectionName()
		{
			if(m_strName.Length > 0)
				return ("trialMax/station/export/objections/" + m_strName);
			else
				return "";

		}// public string GetXmlSectionName()

		/// <summary>This method is called to load the application options from the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file containing the option values</param>
		public void Load(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName()) == false) return;

			//	Read the properties from file
			m_eDelimiter = (TmaxExportDelimiters)(xmlIni.ReadInteger(XMLINI_DELIMITER_KEY, 0));
			m_bAddQuotes = xmlIni.ReadBool(XMLINI_ADD_QUOTES_KEY, m_bAddQuotes);
			m_bSubBinders = xmlIni.ReadBool(XMLINI_SUB_BINDERS_KEY, m_bSubBinders);
			m_eCRLFReplacement = (TmaxExportCRLF)(xmlIni.ReadInteger(XMLINI_CRLF_REPLACEMENT_KEY, (int)m_eCRLFReplacement));
			m_strUserCRLF = xmlIni.Read(XMLINI_USER_CRLF_REPLACEMENT_KEY, m_strUserCRLF);

		}// public void Load(CXmlIni xmlIni)

		/// <summary>This method is called to store the options in the specified XML ini file</summary>
		/// <param name="xmlIni">The initialization file where the options should be stored</param>
		public void Save(CXmlIni xmlIni)
		{
			if(xmlIni.SetSection(GetXmlSectionName(), true, true) == false) return;

			//	Write the properties to file
			xmlIni.Write(XMLINI_DELIMITER_KEY, (int)(this.Delimiter));
			xmlIni.Write(XMLINI_ADD_QUOTES_KEY, m_bAddQuotes);
			xmlIni.Write(XMLINI_SUB_BINDERS_KEY, m_bSubBinders);
			xmlIni.Write(XMLINI_CRLF_REPLACEMENT_KEY, (int)(m_eCRLFReplacement));
			xmlIni.Write(XMLINI_USER_CRLF_REPLACEMENT_KEY, m_strUserCRLF);

		}// public void Save(CXmlIni xmlIni)

		/// <summary>This method get the delimiter text requested by the user</summary>
		/// <returns>the delimiter text</returns>
		public string GetDelimiter()
		{
			return GetDelimiter(this.Delimiter);
		}

		/// <summary>This method converts the enumerated delimiter to the appropriate text</summary>
		/// <param name="eDelimiter">the enumerated delimiter identifier</param>
		/// <returns>the delimiter text</returns>
		static public string GetDelimiter(TmaxExportDelimiters eDelimiter)
		{
			switch(eDelimiter)
			{
				case TmaxExportDelimiters.Comma:

					return ",";

				case TmaxExportDelimiters.Pipe:

					return "|";

				case TmaxExportDelimiters.Tab:
				default:

					return "\t";

			}// switch(eDelimiter)

		}// static public string GetDelimiter(TmaxExportDelimiters eDelimiter)

		/// <summary>This method converts the enumerated substitution to the appropriate text</summary>
		/// <returns>the replacement text</returns>
		public string GetCRLFReplacement()
		{
			if(this.CRLFReplacement == TmaxExportCRLF.User)
				return m_strUserCRLF;
			else
				return GetCRLFReplacement(m_eCRLFReplacement);
		}

		/// <summary>This method converts the enumerated replacement to the appropriate text</summary>
		/// <param name="eReplacement">the enumerated replacement identifier</param>
		/// <returns>the replacement text</returns>
		static public string GetCRLFReplacement(TmaxExportCRLF eReplacement)
		{
			switch(eReplacement)
			{
				case TmaxExportCRLF.HTML:

					return "<BR>";

				case TmaxExportCRLF.Summation:

					char cSummation = (char)0x0b;
					return cSummation.ToString();

				case TmaxExportCRLF.Space:

					return " ";

				case TmaxExportCRLF.Pipe:

					return "|";

				case TmaxExportCRLF.User:
				case TmaxExportCRLF.None:
				default:

					return "";

			}// switch(eReplacement)

		}// static public string GetCRLFReplacement(TmaxExportCRLF eReplacement)

		/// <summary>This method get the text used to display the replacement option</summary>
		/// <param name="eReplacement">the enumerated replacement identifier</param>
		/// <returns>the display text</returns>
		static public string GetDisplayText(TmaxExportCRLF eReplacement)
		{
			switch(eReplacement)
			{
				case TmaxExportCRLF.HTML:

					return "<BR>";

				case TmaxExportCRLF.Summation:

					return "Summation [0xB]";

				case TmaxExportCRLF.Space:
				case TmaxExportCRLF.Pipe:
				case TmaxExportCRLF.User:
				case TmaxExportCRLF.None:
				default:

					return eReplacement.ToString();

			}// switch(eReplacement)

		}// static public string GetCRLFReplacement(TmaxExportCRLF eReplacement)

		#endregion Public Methods

		#region Private Methods

		/// <summary>Called to initialize the object at construction</summary>
		private void Initialize()
		{
			//	Set up the event source
			SetErrorStrings();
			
			m_tmaxEventSource.Name = "Export Objections Options";

		}// private void Initialize()

		/// <summary>This method will populate the local error builder's format string collection</summary>
		private void SetErrorStrings()
		{
			ArrayList aStrings = null;

			if(m_tmaxErrorBuilder != null)
				aStrings = m_tmaxErrorBuilder.FormatStrings;

			if(aStrings == null) return;

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

		/// <summary>The Name used to store the information for this filter in a TrialMax XML configuration file</summary>
		public string Name
		{
			get { return m_strName; }
			set { m_strName = value; }
		}

		/// <summary>Enumerated delimiter identifier</summary>
		public TmaxExportDelimiters Delimiter
		{
			get { return m_eDelimiter; }
			set { m_eDelimiter = value; }
		}

		/// <summary>Enumerated CRLF Replacement identifier</summary>
		public TmaxExportCRLF CRLFReplacement
		{
			get { return m_eCRLFReplacement; }
			set { m_eCRLFReplacement = value; }
		}

		/// <summary>User defined CRLF Replacement</summary>
		public string UserCRLF
		{
			get { return m_strUserCRLF; }
			set { m_strUserCRLF = value; }
		}

		/// <summary>True to add quotes around string values</summary>
		public bool AddQuotes
		{
			get { return m_bAddQuotes; }
			set { m_bAddQuotes = value; }
		}

		/// <summary>True to drill into sub binders</summary>
		public bool SubBinders
		{
			get { return m_bSubBinders; }
			set { m_bSubBinders = value; }
		}

		#endregion Properties

	}//	public class CTmaxExportObjections

}// namespace FTI.Shared.Trialmax
